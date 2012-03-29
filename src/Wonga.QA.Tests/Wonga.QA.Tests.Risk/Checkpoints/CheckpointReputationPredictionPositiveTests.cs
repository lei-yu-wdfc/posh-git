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
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	
    [AUT(AUT.Za)]
	class CheckpointReputationPredictionPositiveTests
	{
		private const RiskMask TestMask = RiskMask.TESTReputationtPredictionPositive;

		private const double ReputationScoreCutoff = 200; //TODO Hardcoded in Risk for now
		private static readonly string[] FactorNames = new [] { "PostcodeInArrears", "LoanNumber", "DeviceCountPostcode", "DeviceDeclineRate" };

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						Drive.Db.SetServiceConfiguration("FeatureSwitch.ZA.ReputationPredictionCheckpoint", "true");
						ResetReputationScoreCutoff();
					}
					break;
				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938")]
		public void CheckpointReputationPredictionPositiveAccept()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithPostcodeInAddress(GetPostcode().ToString())
				.Build();

            var application = ApplicationBuilder.New(customer).WithIovationBlackBox("Accept").WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var actualScore = GetReputationPredictionScore(application);
			Assert.GreaterThan(actualScore, ReputationScoreCutoff);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938")]
		public void CheckpointReputationPredictionPositiveDecline()
		{
			int newCutoff = 800;
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

		[Test, AUT(AUT.Za), JIRA("ZA-1938")]
		public void CheckpointReputationPredictionPositiveCorrectFactorsUsed()
		{
			var customer = CustomerBuilder.New()
				.WithPostcodeInAddress(GetPostcode().ToString())
				.WithEmployer(TestMask)
				.Build();

			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var actualFactorNames = GetFactorNamesUsed(application);
			Assert.AreElementsEqualIgnoringOrder(FactorNames, actualFactorNames);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938")]
		public void CheckpointReputationPredictionPositiveIdentialCustomersHaveEqualScores()
		{
			int postcodeWithHighArrearsRate = GetPostcode();

			var customer1 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcodeWithHighArrearsRate.ToString()).Build();
			var application1 = ApplicationBuilder.New(customer1).Build();

			var score1 = GetReputationPredictionScore(application1);

			var customer2 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcodeWithHighArrearsRate.ToString()).Build();
			var application2 = ApplicationBuilder.New(customer2).Build();

			var score2 = GetReputationPredictionScore(application2);

			Assert.AreEqual(score2, score1);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938")]
		public void CheckpointReputationPredictionPositivePostCodeWithHighArrearsRateLowersScore()
		{
			int postcode = GetPostcode();

			var customer1 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcode.ToString()).Build();
			var application1 = ApplicationBuilder.New(customer1).Build();

			var score1 = GetReputationPredictionScore(application1);

			application1.PutApplicationIntoArrears();

			var customer2 = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(postcode.ToString()).Build();
			var application2 = ApplicationBuilder.New(customer2).Build();

			var score2 = GetReputationPredictionScore(application2);

			Assert.LessThan(score2, score1);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938")]
		public void CheckpointReputationPredictionPositiveTablesUpdateWhenInArrears()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(GetPostcode().ToString()).Build();
			var application = ApplicationBuilder.New(customer).Build();

			var prevInArrears = Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == application.Id).InArrears;
			Assert.IsNotNull(prevInArrears);
			Assert.IsFalse((bool) prevInArrears);

			application.PutApplicationIntoArrears();

			var currentInArrears = Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == application.Id).InArrears;
			Assert.IsNotNull(currentInArrears);
			Assert.IsTrue((bool) currentInArrears);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938"), Explicit]
		public void CheckpointReputationPredictionPositiveTablesUpdateWhenAccountRankIncreases()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithPostcodeInAddress(GetPostcode().ToString()).Build();
			var application = ApplicationBuilder.New(customer).Build();

			var prevAccountRank = Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == application.Id).AccountRank;
			Assert.AreEqual(0, prevAccountRank);

			application.RepayOnDueDate();

			var currentAccountRank = Drive.Db.Risk.RiskIovationPostcodes.Single(a => a.ApplicationId == application.Id).AccountRank;
			Assert.AreEqual(1, currentAccountRank);
		}

		#region Helpers

		private double GetReputationPredictionScore(Application application)
		{
			var db = new DbDriver();
			return (double) (db.Risk.RiskApplications.Where(ra => ra.ApplicationId == application.Id).Join(
				db.Risk.RiskDecisionDatas, ra => ra.RiskApplicationId, dd => dd.RiskApplicationId, (ra, dd) => dd.ValueDouble)).First();
		}

		private IEnumerable<string> GetFactorNamesUsed(Application application)
		{
			var db = new DbDriver();
			return (from ra in db.Risk.RiskApplications
			        where ra.ApplicationId == application.Id
			        join pf in db.Risk.PmmlFactors on ra.RiskApplicationId equals pf.RiskApplicationId
			        join f in db.Risk.Factors on pf.FactorId equals f.FactorId
			        select f.Name).ToArray();

		}

		private void SetReputationScoreCutoff(double cutoff)
		{
			Drive.Db.SetServiceConfiguration("Risk.ReputationScoreCutOff",cutoff.ToString());
		}

		private void ResetReputationScoreCutoff()
		{
			SetReputationScoreCutoff(ReputationScoreCutoff);
		}

		private int GetPostcode()
		{
			return Get.RandomInt(1000, 9999);
		}
		
		#endregion
	}
}