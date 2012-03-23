using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    [Parallelizable(TestScope.All), AUT(AUT.Za)]
	class CheckpointReputationPredictionPositiveTests
	{
		private const RiskMask TestMask = RiskMask.TESTReputationtPredictionPositive;

		private const double ReputationScoreCutoff = 200; //TODO Hardcoded in Risk for now
		private static readonly string[] ExpectedFactorNames = new [] { "PostcodeInArrears", "LoanNumber", "DeviceCountPostcode", "DeviceDeclineRate" };

		private static readonly int PostcodeWithHighArrearsRate = Get.RandomInt(1000, 9999);

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						Drive.Db.SetServiceConfiguration("FeatureSwitch.ZA.ReputationPredictionCheckpoint", "true");
					}
					break;
				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938"), Pending("Work in progress")]
		public void CheckpointReputationPredictionPositiveAccept()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.Build();

            var application = ApplicationBuilder.New(customer).WithIovationBlackBox("Accept").WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var actualScore = GetReputationPredictionScore(application);
			Assert.GreaterThan(actualScore, ReputationScoreCutoff);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938"), Pending("Work in progress")]
		public void CheckpointReputationPredictionPositiveDecline()
		{
			int newCutoff = 800;
			Drive.Db.SetServiceConfiguration("Risk.ReputationScoreCutOff", newCutoff.ToString());

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
				Drive.Db.SetServiceConfiguration("Risk.ReputationScoreCutOff", ReputationScoreCutoff.ToString());
			}
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointReputationPredictionPositiveCorrectFactorsUsed()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.Build();

			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var actualFactorNames = GetFactorNamesUsed(application);
			Assert.AreElementsEqualIgnoringOrder(ExpectedFactorNames, actualFactorNames);
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
		#endregion
	}
}