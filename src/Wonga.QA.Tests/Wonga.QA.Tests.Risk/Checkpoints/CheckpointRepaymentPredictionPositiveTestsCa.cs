using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Ca)]
	public class CheckpointRepaymentPredictionPositiveTestsCa
	{
		private const RiskMask TestMask = RiskMask.TESTRepaymentPredictionPositive;

        CustomerBuilder _customerBuilder;
        Customer _customer;

		private readonly int _scoreCutoffNewUsers = Int32.Parse(Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentScoreNewUsersCutOff").Value);
		
		private string _expectedScorecardNameL0;
		private int _expectedScoreCutoffNewUsers;

		private const string ScorecardNameL0Ca = "<scorecard  and version number for CA>";    //Todo: not available in ops table. Will ask Oleg about this
        private const int ScoreCutoffNewUsersCa = 550;

		[SetUp]
		public void SetUp()
		{
		}

        #region L0 tests

        [Test, AUT(AUT.Ca), Ignore("Not fully implemented, do not run")]
		public void CheckpointRepaymentPredictionPositiveCorrectScorecardUsedL0()
		{
			var scorecardName = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentModelForNewUsers").Value;
			Assert.AreEqual(_expectedScorecardNameL0, scorecardName);
		}

        [Test, AUT(AUT.Ca), Ignore("Not fully implemented, do not run")]
        public void CheckpointRepaymentPredictionPositiveCorrectCutoffL0()
        {
            Assert.AreEqual(_expectedScoreCutoffNewUsers, _scoreCutoffNewUsers);
        }

        [Test, AUT(AUT.Ca), Ignore("Not fully implemented, do not run")]
        public void CheckpointRepaymentPredictionPositiveL0Accept()
        {

            const string forename = "MALCOLM";
            const string surname = "MCCOOL";

            _customerBuilder = CustomerBuilder.New();

            _customerBuilder.ScrubForename(forename);
            _customerBuilder.ScrubSurname(surname);

            _customerBuilder.WithForename(forename);
            _customerBuilder.WithMiddleName("TESTCreditBureauScoreIsAcceptable");
            _customerBuilder.WithSurname(surname);

            _customerBuilder.WithFlatInAddress(null);
            _customerBuilder.WithEmailAddress(Get.GetEmailWithoutPlusChar());
            _customerBuilder.WithEmployer("Wonga");
            _customerBuilder.WithDateOfBirth(new Date(new DateTime(1966, 07, 01), DateFormat.Date));
            _customerBuilder.WithPhoneNumber("111-222-3333");
            _customerBuilder.WithHouseNumberInAddress("1");
            _customerBuilder.WithStreetInAddress("DUNTHORNE CRT");
            _customerBuilder.WithTownInAddress("SCARBORO");
            _customerBuilder.WithPostcodeInAddress("M1B2S9");
            _customerBuilder.WithCountyInAddress(null);
            _customerBuilder.WithDistrictInAddress(null);
            _customer = _customerBuilder.Build();

            var application = ApplicationBuilder.New(_customer).WithLoanTerm(5).WithLoanAmount(100).Build();
            var applicationId = application.Id;

            //var repaymentPredictionScore = GetRepaymentPredictionScore(application);
            //Assert.GreaterThan(repaymentPredictionScore, ScoreCutoffNewUsersCa);
        }

        [Test, AUT(AUT.Ca), Ignore("Not fully implemented, do not run")]
        public void CheckpointRepaymentPredictionPositiveL0Decline()
        {
            const string forename = "DAWN";
            const string surname = "KALINICH";

            _customerBuilder = CustomerBuilder.New();

            _customerBuilder.ScrubForename(forename);
            _customerBuilder.ScrubSurname(surname);

            _customerBuilder.WithForename(forename);
            _customerBuilder.WithMiddleName("TESTCreditBureauScoreIsAcceptable");
            _customerBuilder.WithSurname(surname);

            _customerBuilder.WithFlatInAddress(null);
            _customerBuilder.WithEmailAddress(Get.GetEmailWithoutPlusChar());
            _customerBuilder.WithEmployer("Wonga");
            _customerBuilder.WithDateOfBirth(new Date(new DateTime(1973, 03, 03), DateFormat.Date));
            _customerBuilder.WithPhoneNumber("6471211149");
            _customerBuilder.WithHouseNumberInAddress("1");
            _customerBuilder.WithStreetInAddress("LOCKVIEW");
            _customerBuilder.WithTownInAddress("HUNTSVILLE");
            _customerBuilder.WithPostcodeInAddress("P1H1R3");
            _customerBuilder.WithCountyInAddress(null);
            _customerBuilder.WithDistrictInAddress(null);
            _customer = _customerBuilder.Build();

            var application = ApplicationBuilder.New(_customer).WithLoanTerm(5).WithLoanAmount(100).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            var applicationId = application.Id;

            //var repaymentPredictionScore = GetRepaymentPredictionScore(application);
            //Assert.GreaterThan(repaymentPredictionScore, ScoreCutoffNewUsersCa);
        }

        #endregion

        [TearDown]
        public void TearDown()
        {
            _customer.UpdateForename(Get.GetName());
            _customer.UpdateSurname(Get.GetName());
            _customer.ScrubCcin();
        }

        #region Helpers

        private double GetRepaymentPredictionScore(Application application)
		{
			var db = new DbDriver();
			return (double)(from ra in db.Risk.RiskApplications
							join dd in db.Risk.RiskDecisionDatas
								on ra.RiskApplicationId equals dd.RiskApplicationId
							where ra.ApplicationId == application.Id
							select dd.ValueDouble).First();
		}

		private void AssertFactorsStoredInTable(Application application)
		{

		}

		#endregion
	}
}
