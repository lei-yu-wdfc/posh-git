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
	[AUT(AUT.Ca)]
	public class CheckpointRepaymentPredictionPositiveTestsCa
	{
        //TODO: The L0 scorecard was implemneted incorrectly for CA. 
        //TODO: This will be refactored in the near future. 
        //TODO: Only then can these tests be merged into ZA TESTRepaymentPredictionPositive tests 

        //private const RiskMask TestMask = RiskMask.TESTRepaymentPredictionPositive;
        CustomerBuilder _customerBuilder;
        Customer _customer;
		
		private string _expectedScorecardNameL0;
		private int _expectedScoreCutoffNewUsers;

        private const string ScorecardNameL0Ca = "CARiskScorecard_v_1_1_L0";  
        private const int ScoreCutoffNewUsersCa = 550;

		[SetUp]
		public void SetUp()
		{
            _expectedScorecardNameL0 = ScorecardNameL0Ca;
            _expectedScoreCutoffNewUsers = ScoreCutoffNewUsersCa;
		}

        [Test, AUT(AUT.Ca)]
		public void CheckpointRepaymentPredictionPositiveCorrectScorecardUsedL0()
		{
			var scorecardName = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentModelForNewUsers").Value;
			Assert.AreEqual(_expectedScorecardNameL0, scorecardName);
		}

        [Test, AUT(AUT.Ca)]
        public void CheckpointRepaymentPredictionPositiveCorrectCutoffL0()
        {
            var scoreCutoffNewUsers = Int32.Parse(Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentScoreNewUsersCutOff").Value);
            Assert.AreEqual(_expectedScoreCutoffNewUsers, scoreCutoffNewUsers);
        }

        [Test, AUT(AUT.Ca)]
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

            var repaymentPredictionScore = GetRepaymentPredictionScore(application);
            Assert.GreaterThan(repaymentPredictionScore, ScoreCutoffNewUsersCa);
        }

        [Test, AUT(AUT.Ca)]
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

            var repaymentPredictionScore = GetRepaymentPredictionScore(application);
            Assert.LessThan(repaymentPredictionScore, ScoreCutoffNewUsersCa);
        }

        [Test, AUT(AUT.Ca)]
        public void VerifyLnCustomersAreNotBlockedByScoreCardForAccepted()
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

            var l0application = ApplicationBuilder.New(_customer).WithLoanTerm(5).WithLoanAmount(100).Build();
            Assert.IsTrue(ScoreGeneratedForApplication(l0application));

            l0application.RepayOnDueDate();

            var lNapplication = ApplicationBuilder.New(_customer).WithLoanTerm(10).WithLoanAmount(200).Build();
            Assert.IsFalse(ScoreGeneratedForApplication(lNapplication));
        }

        [Test, AUT(AUT.Ca)]
        public void VerifyTurningOnScorecardDoesNotAfftectLnForAccepted()
        {
            const string forename = "MALCOLM";
            const string surname = "MCCOOL";

            SetRiskUseScorecardModelValue(false);

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

            var l0application = ApplicationBuilder.New(_customer).WithLoanTerm(5).WithLoanAmount(100).Build();
            Assert.IsFalse(ScoreGeneratedForApplication(l0application));

            l0application.RepayOnDueDate();

            SetRiskUseScorecardModelValue(true);

            var lNapplication = ApplicationBuilder.New(_customer).WithLoanTerm(10).WithLoanAmount(200).Build();
            Assert.IsFalse(ScoreGeneratedForApplication(lNapplication));
        }

        [Test, AUT(AUT.Ca)]
        public void VerifyLnCustomersAreNotBlockedByScoreCardForDeclined()
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

            var l0application = ApplicationBuilder.New(_customer).WithLoanTerm(5).WithLoanAmount(100).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            Assert.IsTrue(ScoreGeneratedForApplication(l0application));

            var lNapplication = ApplicationBuilder.New(_customer).WithLoanTerm(10).WithLoanAmount(200).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            Assert.IsTrue(ScoreGeneratedForApplication(lNapplication));
        }

        [Test, AUT(AUT.Ca)]
        public void VerifyTurningOnScorecardDoesNotAfftectLnForDeclined()
        {
            const string forename = "DAWN";
            const string surname = "KALINICH";

            SetRiskUseScorecardModelValue(false);

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

            var l0application = ApplicationBuilder.New(_customer).WithLoanTerm(5).WithLoanAmount(100).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            Assert.IsFalse(ScoreGeneratedForApplication(l0application));

            SetRiskUseScorecardModelValue(true);

            var lNapplication = ApplicationBuilder.New(_customer).WithLoanTerm(10).WithLoanAmount(200).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            Assert.IsTrue(ScoreGeneratedForApplication(lNapplication));
        }

        [TearDown]
        public void TearDown()
        {
            if (_customer != null)
            {
                _customer.UpdateForename(Get.GetName());
                _customer.UpdateSurname(Get.GetName());
                _customer.ScrubCcin();
            }

            SetRiskUseScorecardModelValue(true);
        }

        private double GetRepaymentPredictionScore(Application application)
        {
            var db = new DbDriver();
            return (double) (from rw in db.Risk.RiskWorkflows
                    join rd in db.Risk.RiskDecisionDatas
                        on rw.RiskWorkflowId equals rd.RiskWorkflowId
                    where rw.ApplicationId == application.Id
                    select rd.ValueDouble).First();
        }

        private static bool ScoreGeneratedForApplication(Application application)
        {
            var db = new DbDriver();
            return (from rw in db.Risk.RiskWorkflows
                    join rd in db.Risk.RiskDecisionDatas
                        on rw.RiskWorkflowId equals rd.RiskWorkflowId
                    where rw.ApplicationId == application.Id
                    select rd.ValueDouble).Count() == 1;
        }

	    //verify factors in table

        public static void SetRiskUseScorecardModelValue(bool value)
        {
            var db = new DbDriver();
            var valueString = value.ToString();
            var row = db.Ops.ServiceConfigurations.Single(v => v.Key == "Risk.UseScorecardModel");
            row.Value = valueString;
            db.Ops.SubmitChanges();
        }
	}
}
