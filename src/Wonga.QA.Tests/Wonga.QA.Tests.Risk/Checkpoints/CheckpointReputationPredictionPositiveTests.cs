using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Mocks;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[TestFixture]
    [AUT(AUT.Ca,AUT.Za)]
	class CheckpointReputationPredictionPositiveTests
	{
		private const RiskMask TestMask = RiskMask.TESTReputationtPredictionPositive;

		private double ReputationScoreCutoff
		{
			get { return GetReputationScoreCutoff(); }
		}

		//private const double ReputationScoreCutoff = 200; //TODO Hardcoded in Risk for now
		private static string[] _factorNames;

		private const string DeviceAliasMockString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>  <IovationDataOutput xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Wonga.Iovation\">    <AccountId>{0}</AccountId>    <Details xmlns:d2p1=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">      <d2p1:KeyValueOfstringstring>        <d2p1:Key>device.alias</d2p1:Key>        <d2p1:Value>{1}</d2p1:Value>      </d2p1:KeyValueOfstringstring>      <d2p1:KeyValueOfstringstring>        <d2p1:Key>device.firstseen</d2p1:Key>        <d2p1:Value>2010-06-01 14:43:21</d2p1:Value>      </d2p1:KeyValueOfstringstring>    </Details>    <Reason>It's ok</Reason>    <Result>Allow</Result>    <TrackingNumber>0123456798</TrackingNumber>  </IovationDataOutput>  ";

		private Application _application;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						_factorNames = new[] { "PostcodeInArrears", "LoanNumber", "DeviceCountPostcode" };

						ResetReputationScoreCutoff();

						Drive.Data.Ops.SetServiceConfiguration("Mocks.IovationEnabled", true);
					}
					break;
				case AUT.Ca:
					{
						//LoanNumber is used by all score cards
						_factorNames = new[] { "I_PostCodeNo", "I_Declined_General_Rate", "P_ArrClosedLoansRatio", "LoanNumber" };

						ResetReputationScoreCutoff();

						Drive.Data.Ops.SetServiceConfiguration("Mocks.IovationEnabled", true);
					}
					break;
				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889"), Parallelizable]
		public void CheckpointReputationPredictionPositiveAccept()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithPostcodeInAddress(GetPostcode())
				.Build();

			var iovationBlackBox = CreateIovationMockIfRequired(customer);

			try
			{
				_application = ApplicationBuilder.New(customer).WithIovationBlackBox(iovationBlackBox).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

				var actualScore = GetReputationPredictionScore(_application);
				Assert.GreaterThan(actualScore, ReputationScoreCutoff);
			}

			finally
			{
				DeleteIovationMockIfCustomType(iovationBlackBox);
			}
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889"), Parallelizable]
		public void CheckpointReputationPredictionPositiveDecline()
		{
			var customer = CustomerBuilder.New()
			.WithEmployer(TestMask)
			.Build();

			var application = ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Deny).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

			var actualScore = GetReputationPredictionScore(application);
			Assert.LessThan(actualScore, ReputationScoreCutoff);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889"), DependsOn("CheckpointReputationPredictionPositiveAccept"),]
		public void CheckpointReputationPredictionPositiveCorrectFactorsUsed()
		{
			var actualFactorNames = GetFactorNamesUsed(_application);
			Assert.AreElementsEqualIgnoringOrder(_factorNames, actualFactorNames);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889"), DependsOn("CheckpointReputationPredictionPositiveAccept"),]
		public void CheckpointReputationPredictionPositiveTablesUpdateWhenAccountRankIncreases()
		{
			var prevAccountRank = (int)Drive.Data.Risk.Db.RiskIovationPostcodes.FindByApplicationId(_application.Id).AccountRank;
			Assert.AreEqual(0, prevAccountRank);

			_application.RepayOnDueDate();

			Do.Until(() => Drive.Data.Risk.Db.RiskIovationPostcodes.FindByApplicationId(_application.Id).AccountRank == 1);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938")]
		public void CheckpointReputationPredictionPositivePostCodeWithHighArrearsRateLowersScore()
		{
			//this test does not make sense for CA as if the number of arrears in the post code is too high the app will be declined
			string postcode = GetPostcode();
			var customer1 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcode).Build();
			var customer2 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcode).Build();

			var iovationBlackBox1 = CreateIovationMockIfRequired(customer1);
			var iovationBlackBox2 = CreateIovationMockIfRequired(customer2);

			try
			{
				var application1 = ApplicationBuilder.New(customer1).WithIovationBlackBox(iovationBlackBox1).Build();
				var score1 = GetReputationPredictionScore(application1);

				application1.PutIntoArrears();

				var application2 = ApplicationBuilder.New(customer2).WithIovationBlackBox(iovationBlackBox2).Build();
				var score2 = GetReputationPredictionScore(application2);

				Assert.LessThanOrEqualTo(score2, score1);
			}

			finally
			{
				DeleteIovationMockIfCustomType(iovationBlackBox1);
				DeleteIovationMockIfCustomType(iovationBlackBox2);
			}
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889")]
		public void CheckpointReputationPredictionPositiveTablesUpdateWhenInArrears()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(GetPostcode()).Build();
			var iovationBlackBox = CreateIovationMockIfRequired(customer);

			try
			{
				var application = ApplicationBuilder.New(customer).WithIovationBlackBox(iovationBlackBox).Build();

				var prevInArrears = (bool)Drive.Data.Risk.Db.RiskIovationPostcodes.FindByApplicationId(application.Id).InArrears;
				Assert.IsNotNull(prevInArrears);
				Assert.IsFalse((bool)prevInArrears);

				application.PutIntoArrears();

				Do.Until(() => (bool)Drive.Data.Risk.Db.RiskIovationPostcodes.FindByApplicationId(application.Id).InArrears);
			}

			finally
			{
				DeleteIovationMockIfCustomType(iovationBlackBox);
			}
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1889") ]
		[Row(false, Order = 1)]
		[Row(true, Order = 2)]
		public void CheckpointReputationPredictionPositiveCheckL0ExecutionForReputationScoreCardUsageConfiguration(bool useScoreCard)
		{
			bool currentValue = Drive.Data.Ops.SetServiceConfiguration(GetUseReputationScoreModelKeyName(), useScoreCard);

			try
			{
				//don't use mask so that the workflow builder is run!
				var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();

				var application = ApplicationBuilder.New(customer).WithoutExpectedDecision().Build();

				AssertCheckpointExecution(application.Id, RiskCheckpointDefinitionEnum.ReputationPredictionPositive, useScoreCard);
				AssertVerificationExecution(application.Id, RiskVerificationDefinitions.ReputationPredictionPositiveVerification, useScoreCard);
			}
			finally
			{
				Drive.Data.Ops.SetServiceConfiguration(GetUseReputationScoreModelKeyName(), currentValue);
			}
		}


		[Test, AUT(AUT.Ca), JIRA("CA-2218")]
		public void ReputationScoreShouldNotRunForBrandNewDevice()
		{
			string uniqueAlias = Guid.NewGuid().ToString("N");

			IovationResponseBuilder.New()
				.ForBlackBox(IovationMockResponse.AllowUniqueDevice)
				.UseDeviceAlias(uniqueAlias)
				.OnResponseBasedOn(IovationMockResponse.Allow);

			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithPostcodeInAddress(GetPostcode())
				.Build();

			var application = ApplicationBuilder.New(customer)
				.WithIovationBlackBox(IovationMockResponse.AllowUniqueDevice)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted)
				.Build();

			AssertNoReputationPredictionScore(application);
		}

		#region Helpers

		private string GetUseReputationScoreModelKeyName()
		{
			switch (Config.AUT)
			{
				case AUT.Ca:
					return "FeatureSwitch.CA.ReputationPredictionCheckpoint";

				case AUT.Za:
					return "FeatureSwitch.ZA.ReputationPredictionCheckpoint";

				default:
					throw new NotImplementedException();
			}
		}

		private double GetReputationPredictionScore(Application application)
		{
			var riskWorkflowId = (int)Do.Until(() => Drive.Data.Risk.Db.RiskWorkflows.FindByApplicationId(application.Id)).RiskWorkflowId;
			var score = (double)Do.Until(() => Drive.Data.Risk.Db.RiskDecisionData.FindByRiskWorkflowId(riskWorkflowId).ValueDouble);

			return score;
		}

		private void AssertNoReputationPredictionScore(Application application)
		{
			var riskWorkflowId = (int)Do.Until(() => Drive.Data.Risk.Db.RiskWorkflows.FindByApplicationId(application.Id)).RiskWorkflowId;
			Assert.IsNull(Drive.Data.Risk.Db.RiskDecisionData.FindByRiskWorkflowId(riskWorkflowId));
		}

		private IEnumerable<string> GetFactorNamesUsed(Application application)
		{
			int workflowId = (int)Drive.Data.Risk.Db.RiskWorkflows.FindByApplicationId(application.Id).RiskWorkflowId;

			var pmmlFactorIds = ((List<PmmlFactorEntity>)Drive.Data.Risk.Db.PmmlFactors.FindAllByRiskWorkflowId(workflowId).ToList<PmmlFactorEntity>()).Select(a => a.FactorId);
			var factorNames = ((List<FactorEntity>)Drive.Data.Risk.Db.Factors.FindAllByFactorId(pmmlFactorIds).ToList<FactorEntity>()).Select(a => a.Name).ToList();

			return factorNames;
		}

		private void SetReputationScoreCutoff(double cutoff)
		{
			Drive.Data.Ops.SetServiceConfiguration("Risk.ReputationScoreCutOff", cutoff);
		}

		private void ResetReputationScoreCutoff()
		{
			SetReputationScoreCutoff(ReputationScoreCutoff);
		}

		private string GetPostcode()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					return Get.GetPostcode();

				case AUT.Ca:
					return GetCaPostCode();

				default:
					throw new NotImplementedException();
			}
		}

		private string GetCaPostCode()
		{
			ProvinceEnum province = Get.RandomEnumOf(ProvinceEnum.AB, ProvinceEnum.BC, ProvinceEnum.ON);
			var postCodeStart = GetCaPostCodeInitialChar(province);

			return string.Format("{0}{1}{2}{3}{4}{5}",
				postCodeStart,
				Get.RandomInt(0, 10),
				Get.RandomString(1).ToUpperInvariant(),
				Get.RandomInt(0, 10),
				Get.RandomString(1).ToUpperInvariant(),
				Get.RandomInt(0, 10));
		}

		private static char GetCaPostCodeInitialChar(ProvinceEnum province)
		{
			char postCodeStart = '\0';
			switch (province)
			{
				case ProvinceEnum.AB:
					postCodeStart = 'T';
					break;
				case ProvinceEnum.BC:
					postCodeStart = 'V';
					break;

				case ProvinceEnum.ON:
					postCodeStart = 'K';
					break;

				default:
					throw new NotImplementedException();
			}
			return postCodeStart;
		}

		private void AssertCheckpointExecution(Guid applicationId, RiskCheckpointDefinitionEnum checkpoint, bool executed)
		{
			var checkpoints = Drive.Db.GetCheckpointDefinitionsForApplication(applicationId);
			Assert.AreEqual(executed, checkpoints.Any(c => c.Name == Get.EnumToString(checkpoint)));
		}

		private void AssertVerificationExecution(Guid applicationId, RiskVerificationDefinitions verification, bool executed)
		{
			var verifications = Drive.Db.GetVerificationDefinitionsForApplication(applicationId);
			Assert.AreEqual(executed, verifications.Any(v => v.Name == Get.EnumToString(verification)));
		}

		private string CreateIovationMockIfRequired(Customer customer)
		{
			string type = string.Empty;

			switch (Config.AUT)
			{
				case AUT.Ca:
					//in CA score is only calculated if the device already exists in the DB
					type = IovationMockResponse.Allow.ToString();
					break;

				case AUT.Za:
					type = customer.Id.ToString();

					var response = String.Format(DeviceAliasMockString, customer.Id, Get.RandomLong(1000000000, 9999999999).ToString());
					Drive.Data.QaData.Db.IovationDataOutput.Insert(Type: type, Response: response, WaitTimeInSeconds: 0);
					break;

				default:
					throw new NotImplementedException();
			}

			return type;
		}

		private void DeleteIovationMockIfCustomType(string type)
		{
			//just clean up the iovation response if it is not one of the predefined values
			IovationMockResponse response;
			if (!IovationMockResponse.TryParse(type, out response))
			{
				Drive.Data.QaData.Db.IovationDataOutput.DeleteByType(type);
			}
		}

		private double GetReputationScoreCutoff()
		{
			switch (Config.AUT)
			{
				case AUT.Ca:
					return 610;

				case AUT.Za:
					return 200;

				default:
					throw new NotImplementedException();
			}

		}


		#endregion
	}
}