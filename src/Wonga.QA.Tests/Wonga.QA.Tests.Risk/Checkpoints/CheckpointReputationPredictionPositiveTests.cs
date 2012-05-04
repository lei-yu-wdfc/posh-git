using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	
    [AUT(AUT.Za, AUT.Ca)]
	class CheckpointReputationPredictionPositiveTests
	{
		private const RiskMask TestMask = RiskMask.TESTReputationtPredictionPositive;
		private const double ReputationScoreCutoff = 200; //TODO Hardcoded in Risk for now
    	private static string[] _factorNames;

    	[FixtureSetUp]
		public void FixtureSetUp()
		{
			Drive.Data.Ops.SetServiceConfiguration(GetUseReputationScoreModelKeyName(), true);

			switch (Config.AUT)
			{
				case AUT.Za:
					{
						_factorNames = new[] { "PostcodeInArrears", "LoanNumber", "DeviceCountPostcode", "DeviceDeclineRate" };

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

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889")]
		public void CheckpointReputationPredictionPositiveAccept()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithPostcodeInAddress(GetPostcode())
				.Build();

            var application = ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Allow).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var actualScore = GetReputationPredictionScore(application);
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

				var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

				var actualScore = GetReputationPredictionScore(application);
				Assert.LessThan(actualScore,newCutoff);
			}

			finally
			{
				ResetReputationScoreCutoff();
			}
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889")]
		public void CheckpointReputationPredictionPositiveCorrectFactorsUsed()
		{
			var customer = CustomerBuilder.New()
				.WithPostcodeInAddress(GetPostcode())
				.WithEmployer(TestMask)
				.Build();

			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var actualFactorNames = GetFactorNamesUsed(application);
			Assert.AreElementsEqualIgnoringOrder(_factorNames, actualFactorNames);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889")]
		public void CheckpointReputationPredictionPositiveIdentialCustomersHaveEqualScores()
		{
			string postcodeWithHighArrearsRate = GetPostcode();

			var customer1 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcodeWithHighArrearsRate).Build();
			var application1 = ApplicationBuilder.New(customer1).Build();

			var score1 = GetReputationPredictionScore(application1);

			var customer2 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcodeWithHighArrearsRate).Build();
			var application2 = ApplicationBuilder.New(customer2).Build();

			var score2 = GetReputationPredictionScore(application2);

			Assert.AreEqual(score2, score1);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889")]
		public void CheckpointReputationPredictionPositivePostCodeWithHighArrearsRateLowersScore()
		{
			string postcode = GetPostcode();

			var customer1 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcode).Build();
			var application1 = ApplicationBuilder.New(customer1).Build();

			var score1 = GetReputationPredictionScore(application1);

			application1.PutApplicationIntoArrears();

			var customer2 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcode).Build();
			var application2 = ApplicationBuilder.New(customer2).Build();

			var score2 = GetReputationPredictionScore(application2);

			Assert.LessThan(score2, score1);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889")]
		public void CheckpointReputationPredictionPositiveTablesUpdateWhenInArrears()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(GetPostcode()).Build();
			var application = ApplicationBuilder.New(customer).Build();

			var prevInArrears = Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == application.Id).InArrears;
			Assert.IsNotNull(prevInArrears);
			Assert.IsFalse((bool) prevInArrears);

			application.PutApplicationIntoArrears();

			var currentInArrears = Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == application.Id).InArrears;
			Assert.IsNotNull(currentInArrears);
			Assert.IsTrue((bool) currentInArrears);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-1938", "CA-1889")]
		public void CheckpointReputationPredictionPositiveTablesUpdateWhenAccountRankIncreases()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(GetPostcode()).Build();
			var application = ApplicationBuilder.New(customer).Build();

			var prevAccountRank = Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == application.Id).AccountRank;
			Assert.AreEqual(0, prevAccountRank);

			application.RepayOnDueDate();

			var currentAccountRank = Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == application.Id).AccountRank;
			Assert.AreEqual(1, currentAccountRank);
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

				var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Pending).Build();

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
					return "Risk.UseReputationModel";

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
					return Get.RandomInt(1000, 9999).ToString();

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

    	#endregion
	}
}