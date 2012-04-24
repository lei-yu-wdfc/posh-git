using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.UI;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using EmploymentStatusEnum = Wonga.QA.Framework.Api.EmploymentStatusEnum;

namespace Wonga.QA.Tests.Ui
{
    public class TagCloudTests : UiTest
    {
        Dictionary<int, string> tagCloudTexts = new Dictionary<int, string> 
	    {
	     {2, "Request Credit\r\nChange Promise Date\r\nView Loan Details\r\nRepay"},
         {3, "Request Credit\r\nChange Promise Date\r\nView Loan Details\r\nRepay"},
         {4, "Request Credit\r\nChange Promise Date\r\nView Loan Details\r\nRepay"}, //Change Promise Date is disabled
         {5, "Change Promise Date\r\nView Loan Details\r\nRepay"},
         {6, "Change Promise Date\r\nView Loan Details\r\nRepay"},
         {7, "View Loan Details\r\nRepay\r\nChange Promise Date"}, //Change Promise Date is disabled
         {8, ""}, 
         {9, "Repay\r\nAdd Payment Card\r\nView Loan Details"},
         {10, "Repay\r\nAdd Payment Card"},
         {11, "Repay\r\nSetup Repayment Plan\r\nAdd Payment Card"},
         {12, "Repay\r\nSetup Repayment Plan\r\nAdd Payment Card"},
         {13, "Repay\r\nSetup Repayment Plan\r\nAdd Payment Card"},
         {14, "Repay\r\nAdd Payment Card"},
         {15, "Repay\r\nAdd Payment Card"},
         {16, "Repay\r\nAdd Payment Card"},
         {17, ""},
         {19, ""},
         {20, ""},
         {21, ""},
	    };

        [Test, AUT(AUT.Uk), JIRA("UK-785", "UK-1614"), Pending("Fails due to bug UK-1614")]
        public void TagCloudScenario1A()
        {
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var aPage = journey.ApplyForLoan(loanAmount, days)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails()
                .FillAccountDetails();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
        }
        
        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void TagCloudScenario1B()
        // Ln journey
        {
            string email = Get.RandomEmail();
            const decimal trustRating = 400.00M;
            var applicationId = Guid.NewGuid();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var accountId = customer.Id;

            var setupData = new AccountSummarySetupFunctions();

            setupData.Scenario01Setup(accountId, applicationId, trustRating);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(1, int.Parse(response.Values["ScenarioId"].Single()));

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
        }


        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario02() { TagCloud(2, 2); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario03() { TagCloud(3, 3); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario04() { TagCloud(4, 10); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario05() { TagCloud(5, 2); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario06() { TagCloud(6, 3); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario07() { TagCloud(7, 10); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario08() { TagCloud(8, 10); }

        //[Test, AUT(AUT.Uk), JIRA("UK-785")]
        //public void TagCloudScenario09() { TagCloud(9, 3); } // not ready

        //[Test, AUT(AUT.Uk), JIRA("UK-785")]
        //public void TagCloudScenario10() { TagCloud(10); } // not ready

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario11() { TagCloud(11, 14); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario12() { TagCloud(12, 44); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario13() { TagCloud(13, 64); }

        //[Test, AUT(AUT.Uk), JIRA("UK-785")]
        //public void TagCloudScenario14() { TagCloud(14, 20); } // not ready

        //[Test, AUT(AUT.Uk), JIRA("UK-785")]
        //public void TagCloudScenario15() { TagCloud(15, 11); } // not ready

        //[Test, AUT(AUT.Uk), JIRA("UK-785")]
        //public void TagCloudScenario16() { TagCloud(16, 11); } // not ready

        [Test, AUT(AUT.Uk), JIRA("UK-785", "UK-1624"), Pending("Fails due to bug UK-1624")]
        public void TagCloudScenario17A()
        {
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var aPage = journey.ApplyForLoan(loanAmount, days)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-785", "UK-1624"), Pending("Fails due to bug UK-1624")]
        public void TagCloudScenario17B()
        {
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var aPage = journey.ApplyForLoan(loanAmount, days)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
        }
        //[Test, AUT(AUT.Uk), JIRA("UK-785")]
        //public void TagCloudScenario19() { TagCloud(19, 0); } // not ready

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario20() { TagCloud(20, 1); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario21()
        {
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var aPage = journey.ApplyForLoan(loanAmount, days)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
        }


        private void TagCloud(int scenarioId, int daysShift)
        {
            // Create a customer
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);
            Customer customer;
            if (scenarioId == 20) customer = CustomerBuilder.New().WithEmailAddress(email).WithEmployerStatus(EmploymentStatusEnum.Unemployed.ToString()).WithMiddleName(RiskMask.TESTEmployedMask).Build();
            else customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            // Create a loan
            Application application;
            if (scenarioId < 5) application = ApplicationBuilder.New(customer).Build();
            else if ((scenarioId >= 5) && (scenarioId <= 7)) application = ApplicationBuilder.New(customer).WithLoanAmount(400).Build();
            //else if (scenarioId == 17) application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Pending).Build(); 
            else if (scenarioId == 20) application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            else application = ApplicationBuilder.New(customer).Build();

            // Rewind application dates
            ApplicationEntity applicationEntity = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            Drive.Db.RewindApplicationDates(applicationEntity, riskApplication, daysShiftSpan);

            if (scenarioId == 8) application = application.RepayOnDueDate(); // Repay a loan

            /* // Create repayment plan
             * if ((scenarioId >= 14) && (scenarioId <= 16)) 
            {
                Drive.Msmq.Payments.Send(new CreateExtendedRepaymentArrangementCommand {
                                                AccountId = customer.Id,
                                                ApplicationId = application.Id,
                                                EffectiveBalance = application.GetBalance(),
                                                RepaymentAmount = application.GetBalance(),
                                                RepaymentDetails = new[]
                                                                    {
                                                                        new ArrangementDetail{Amount = application.GetBalance(), Currency = CurrencyCodeIso4217Enum.GBP, DueDate = DateTime.Today.AddDays(7)}
                                                                    }
                                            });
                
                } */

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check Tag Cloud is displayed/correct
            if ((scenarioId == 8) || (scenarioId == 20))
            {
                Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
                return;
            }
            string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);
        }
    }
}