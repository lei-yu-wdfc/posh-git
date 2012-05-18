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
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Data.Enums.Risk;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[TestFixture, Parallelizable(TestScope.Self)]
	class CheckpointReputationPredictionPositiveTests
	{
		private const RiskMask TestMask = RiskMask.TESTReputationtPredictionPositive;
		private const double ReputationScoreCutoff = 200; //TODO Hardcoded in Risk for now
		private static string[] _factorNames;

		private const string OriginalDeviceAlias = "12321234123";
		private const string DeviceAliasMockString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>  <IovationDataOutput xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Wonga.Iovation\">    <AccountId>8b415fed-ac43-4e2b-8b95-b7399de0f562</AccountId>    <Details xmlns:d2p1=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">      <d2p1:KeyValueOfstringstring>        <d2p1:Key>device.alias</d2p1:Key>        <d2p1:Value>12321234123</d2p1:Value>      </d2p1:KeyValueOfstringstring>      <d2p1:KeyValueOfstringstring>        <d2p1:Key>device.firstseen</d2p1:Key>        <d2p1:Value>2010-06-01 14:43:21</d2p1:Value>      </d2p1:KeyValueOfstringstring>    </Details>    <Reason>It's ok</Reason>    <Result>Allow</Result>    <TrackingNumber>0123456798</TrackingNumber>  </IovationDataOutput>  ";

		private Application l0Application;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			Drive.Data.Ops.SetServiceConfiguration(GetUseReputationScoreModelKeyName(), true);

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

						Drive.Data.Ops.SetServiceConfiguration("Mocks.IovationEnabled", true);
					}
					break;
				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			ResetDeviceAliasMock();
		}

		[SetUp]
		public void SetUp()
		{
			RandomiseDeviceAliasMock();
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889")]
		public void CheckpointReputationPredictionPositiveAccept()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithPostcodeInAddress(GetPostcode())
				.Build();

			l0Application = ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Allow).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var actualScore = GetReputationPredictionScore(l0Application);
			Assert.GreaterThan(actualScore, ReputationScoreCutoff);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889")]
		public void CheckpointReputationPredictionPositiveDecline()
		{
			int newCutoff = 999;
			SetReputationScoreCutoff(newCutoff);

			try
			{
				var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.Build();

				var application = ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Deny).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

				var actualScore = GetReputationPredictionScore(application);
				Assert.LessThan(actualScore, newCutoff);
			}

			finally
			{
				ResetReputationScoreCutoff();
			}
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889"), DependsOn("CheckpointReputationPredictionPositiveAccept")]
		public void CheckpointReputationPredictionPositiveCorrectFactorsUsed()
		{
			var actualFactorNames = GetFactorNamesUsed(l0Application);
			Assert.AreElementsEqualIgnoringOrder(_factorNames, actualFactorNames);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889"), DependsOn("CheckpointReputationPredictionPositiveAccept"), Pending("Why doesn't customer go into arrears?")]
		public void CheckpointReputationPredictionPositiveTablesUpdateWhenAccountRankIncreases()
		{
			var prevAccountRank = Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == l0Application.Id).AccountRank;
			Assert.AreEqual(0, prevAccountRank);

			l0Application.RepayOnDueDate();

			Do.Until(() => Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == l0Application.Id).AccountRank == 1);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889")]
		public void CheckpointReputationPredictionPositivePostCodeWithHighArrearsRateLowersScore()
		{
			string postcode = GetPostcode();

			var customer1 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcode).Build();
			var application1 = ApplicationBuilder.New(customer1).WithIovationBlackBox(IovationMockResponse.Allow).Build();

			var score1 = GetReputationPredictionScore(application1);

			application1.PutApplicationIntoArrears();

			RandomiseDeviceAliasMock();

			var customer2 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcode).Build();
			var application2 = ApplicationBuilder.New(customer2).WithIovationBlackBox(IovationMockResponse.Allow).Build();

			var score2 = GetReputationPredictionScore(application2);

			Assert.LessThan(score2, score1);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889"), Pending("Why doesn't customer go into arrears?")]
		public void CheckpointReputationPredictionPositiveTablesUpdateWhenInArrears()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(GetPostcode()).Build();
			var application = ApplicationBuilder.New(customer).Build();

			var prevInArrears = Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == application.Id).InArrears;
			Assert.IsNotNull(prevInArrears);
			Assert.IsFalse((bool)prevInArrears);

			application.PutApplicationIntoArrears();

			Do.Until(() => Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == application.Id).InArrears);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1889")]
		[Row(false)]
		[Row(true)]
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

		private void RandomiseDeviceAliasMock()
		{
			var deviceAlias = Get.RandomLong(1000000000, 9999999999).ToString(CultureInfo.InvariantCulture);
			SetDeviceAliasMock(deviceAlias);
		}

		private void ResetDeviceAliasMock()
		{
			var mock = Drive.Data.QaData.Db.IovationDataOutput.FindByType("Allow");
			mock.Response = DeviceAliasMockString;
			Drive.Data.QaData.Db.IovationDataOutput.Update(mock);
		}

		private void SetDeviceAliasMock(string deviceAlias)
		{
			var mock = Drive.Data.QaData.Db.IovationDataOutput.FindByType("Allow");
			mock.Response = DeviceAliasMockString.Replace(OriginalDeviceAlias, deviceAlias);
			Drive.Data.QaData.Db.IovationDataOutput.Update(mock);
		}

		#endregion
	}
}