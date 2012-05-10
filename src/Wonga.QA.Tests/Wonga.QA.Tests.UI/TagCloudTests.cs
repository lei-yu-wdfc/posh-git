using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using CreateScheduledPaymentRequestCommand = Wonga.QA.Framework.Msmq.CreateScheduledPaymentRequestCommand;
using EmploymentStatusEnum = Wonga.QA.Framework.Api.EmploymentStatusEnum;

namespace Wonga.QA.Tests.Ui
{
    public class TagCloudTests : UiTest
    {
        Dictionary<int, string> tagCloudTexts = new Dictionary<int, string> 
	    {
	     {2, "Request Credit\r\nChange Promise Date\r\nView Loan Details\r\nRepay"}, //Change Promise Date should be only a tooltip
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


        [Test, AUT(AUT.Uk), JIRA("UK-785", "UK-1737")]
        public void TagCloudScenario02() { TagCloud(2, 2); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario03() { TagCloud(3, 3); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        [Row(4, 9)]
        public void TagCloudScenario04(int scenarioId, int dasyShift) { TagCloud(scenarioId, dasyShift); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario05() { TagCloud(5, 2); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario06() { TagCloud(6, 3); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario07() { TagCloud(7, 10); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario08() { TagCloud(8, 10); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario09() 
        {
            const int scenarioId = 9;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var accountId = customer.Id;
            var bankAccountId = customer.BankAccountId;
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario09Setup(requestId2, requestId1, accountId, paymentCardId, appId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);

            ChangeWantToRepayBox(customer, customer.GetApplication());
        } 

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario10() 
        {
            const int scenarioId = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var accountId = customer.Id;
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario10Setup(requestId1, requestId2, appId, bankAccountId, accountId, paymentCardId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);

            ChangeWantToRepayBox(customer, customer.GetApplication());
        } 

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario11() { TagCloud(11, 14); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario12() { TagCloud(12, 44); }

        [Test, AUT(AUT.Uk), JIRA("UK-785")]
        public void TagCloudScenario13() { TagCloud(13, 64); }

        [Test, AUT(AUT.Uk), JIRA("UK-785"), Pending("Awating Repayment Arrangment Functionality.")]
        public void TagCloudScenario14()
        {
            var scenarioId = 14;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var accountId = customer.Id;
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario14Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-785"), Pending("Awating Repayment Arrangment Functionality.")]
        public void TagCloudScenario15()
        {
            const int scenarioId = 15;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var accountId = customer.Id;
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario15Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-785"), Pending("Awating Repayment Arrangment Functionality.")]
        public void TagCloudScenario16()
        {
            const int scenarioId = 16;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var accountId = customer.Id;
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario15Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);
        } 

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

        [Test, AUT(AUT.Uk), JIRA("UK-785"), Pending("Waiting for implementation of agreement cancellation process.")]
        public void TagCloudScenario19() { TagCloud(19, 0); } 

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
            else if (scenarioId == 20) application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            else application = ApplicationBuilder.New(customer).Build();

            // Rewind application dates
            if (daysShift != 0)
            {
                ApplicationEntity applicationEntity = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
                RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
                TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
                Drive.Db.RewindApplicationDates(applicationEntity, riskApplication, daysShiftSpan);
            }

            if (scenarioId == 8) application = application.RepayOnDueDate(); // Repay a loan

            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            if (scenarioId == 11 || scenarioId == 12 || scenarioId == 13)
            {
                // Send command to create scheduled payment request
                Drive.Msmq.Payments.Send(new Framework.Msmq.CreateScheduledPaymentRequestCommand() { ApplicationId = application.Id, RepaymentRequestId = requestId1, });
                Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = application.Id, RepaymentRequestId = requestId2, });
            }
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
            
            if (actualTagCloudText.IndexOf("Repay") > 0)
            {
                ChangeWantToRepayBox(customer, application);
            }
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1827")]
        public void ChangeWantToRepayBox(Customer customer, Application application)
        {

            var AmountToRepayMinimum = 5;
            
            string email = customer.Email;
            DateTime todayDate = DateTime.Now;

            // Open Repay Request page
            var mySummaryPage = new MySummaryPage(Client);
            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            ApiResponse response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = application.Id });
            TimeSpan daysToNextDueDay = Convert.ToDateTime(response.Values["NextDueDate"].Single()) - DateTime.Today;


            // You currently owe
            var expectedOweToday = Convert.ToDecimal(response.Values["BalanceToday"].Single());
            string sExpectedOweToday = String.Format("{0:0.00}", expectedOweToday);

            // TBD - change values in the Want to Repay box
            var amountToRepayList = new List<decimal> { AmountToRepayMinimum, Convert.ToInt16(expectedOweToday - 1), expectedOweToday };
            var random = new Random();
            amountToRepayList.Add(random.Next(AmountToRepayMinimum, Convert.ToInt16(expectedOweToday - 2)));

            string sActualOweToday;
            // decimal expectedWantToRepay;
            string sExpectedWantToRepay;
            //string sActualWantToRepay;


            // Check minumum and maximum values in the "Want to Repay" box
            var sliders = new SmallRepaySlidersElement(requestPage) { HowMuch = "1" };
            Assert.AreEqual(AmountToRepayMinimum.ToString("#"), sliders.HowMuch);

            sliders = new SmallRepaySlidersElement(requestPage) { HowMuch = "1000" };
            Assert.AreEqual(sExpectedOweToday, sliders.HowMuch);


            foreach (decimal amountToRepay in amountToRepayList)
            {
                requestPage.WantToRepayBox = amountToRepay.ToString("#.##");
                Thread.Sleep(2000);

                // Currently Owe
                sActualOweToday = requestPage.OweToday.TrimStart('£');
                Assert.AreEqual(sExpectedOweToday, sActualOweToday, "Currently Owe is wrong.");

                // Remainder to repay = Amount Owed - Repay Amount
                var expectedReminderToRepay = expectedOweToday - amountToRepay;
                string sExpectedReminderToRepay = String.Format("{0:0.00}", expectedReminderToRepay);
                string sActualReminderToRepay = requestPage.RemainderAmount.TrimStart('£');
                //Assert.AreEqual(sExpectedReminderToRepay, sActualReminderToRepay, "Reminder Amount is wrong.");

                // Repay Total in the Read Me message
                sExpectedWantToRepay = String.Format("{0:0.00}", amountToRepay);
                string sActualRepayTotal = requestPage.RepayTotal.TrimStart('£');
                //Assert.AreEqual(sExpectedWantToRepay, sActualRepayTotal, "Repay Total in the Read Me message is wrong.");


                // Loan Period Clairification (in N days)
                var sExpectedLoanPeriodClarification = "(in " + daysToNextDueDay.TotalDays.ToString("#") + " days)";
                var sActualLoanPeriodClarification = requestPage.LoanPeriodClarification;
                if ((sExpectedWantToRepay != String.Format("{0:0.00}", expectedOweToday)) && (daysToNextDueDay.Days > 0))
                {
                    Assert.AreEqual(sExpectedLoanPeriodClarification, sActualLoanPeriodClarification, "Wrong oan Period Clarification, <in N days>");
                    Assert.IsTrue(requestPage.IsLoanPeriodClarificationDisplayed);
                }
                else
                {
                    Assert.IsFalse(requestPage.IsLoanPeriodClarificationDisplayed);
                }
            }
        }
    }
}