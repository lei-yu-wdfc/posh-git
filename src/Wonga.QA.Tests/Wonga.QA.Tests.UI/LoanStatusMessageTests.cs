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
using Wonga.QA.Framework.Msmq;
using CreateScheduledPaymentRequestCommand = Wonga.QA.Framework.Msmq.CreateScheduledPaymentRequestCommand;
using EmploymentStatusEnum = Wonga.QA.Framework.Msmq.EmploymentStatusEnum;

namespace Wonga.QA.Tests.Ui
{
    public class LoanStatusMessageTests : UiTest
    {
        Dictionary<int, string> loanStatusMessages = new Dictionary<int, string> 
	    {
	    {2, @"If you would like to change your repayment date it's too early to do it just yet, but you can request a new one from {date extensions available}. You can set a handy reminder to do that below. Please bear in mind that you will need to pay any interest and fees up to that point, in order for a new date to be approved."},
        {3, @"If you would like to change your promised repayment date you can do so here. Please note you can only extend your promise date a maximum of three times and will need to pay any interest and fees up to that point each time you extend, in order to have any request approved."},
        {4, @"Please also remember you have promised to repay £{total to repay 300.00} on {promise date}, when you simply need to ensure the funds are available in the bank account linked to your primary debit card. Changing your promise date isn't possible at this point, so we look forward to collecting your payment and then being of service again in the future."},
        {5, @"If you would like to change your repayment date it's too early to do it just yet, but you can request a new one from {date extensions available}. You can set a handy reminder to do it below. Please bear in mind that you will need to pay any interest and fees up to that point, in order to have your request for a change of promise date approved."},
        {6, @"If you would like to change your promised repayment date you can do so here. Please note you can only extend your promise date a maximum of three times and will need to pay any interest and fees up to that point each time you extend, in order to have any request approved."},
        {7, @"Changing your promise date isn't possible at this point, so we look forward to collecting your full payment and then being of service again in the future. Thanks for using Wonga!"},
        {9, @"We understand genuine mistakes happen so we hope you can make this payment today and save yourself further costs. If the balance isn't cleared by 5pm today, however, you will incur a missed payment fee of £20, which is the last thing we want to happen! Please click repay now to settle your balance. You can add a new debit card if you need to. If you are unable to pay in full today, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {10, @"You have unfortunately incurred a missed payment fee of £20 and interest continues to accrue. Please click repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. If you are unable to pay in full today, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {11, @"Please act now to avoid incurring further interest, which continues to accrue. Please click repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of four months. If this doesn't work for you, you should call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {12, @"Please act now to avoid incurring further interest, which continues to accrue, and potential negative entries on your credit file. Please click repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of six months. If this doesn't work for you, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {13, @"Please take action today to avoid incurring further interest, which continues to accrue, and potential negative entries on your credit file. Click Repay now to settle your balance and bring your account back into line. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of six months. If this doesn't work for you, please call our friendly collections team straight away on 0844 842 9109. If you choose not to deal with this matter immediately, we may need to take more formal steps to recover the balance owed."},
        {15, @"Please make this payment online right away, or call our automated payment line on 0207 183 0063 to resolve this situation quickly. If you don't make this payment by {deadline for grace period} your plan will be cancelled automatically and interest will start to accrue again, which is the last thing we want to happen! Please act today to avoid further costs."},
        {16, @"Your account is seriously in arrears and interest is again accruing . Your current balance is £{total balance today £230.45}. Please click repay now to settle in full and rectify this situation. If you are unable to pay in full today, please call our collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday. If you choose not to deal with this matter immediately, we may need to take more formal steps to recover the balance owed."},
        {17, @"Your application is in the final stages of our approval process. We hate to keep you waiting, but, on this rare occasion, we need to check a few more details. There's no need to contact us or do anything and you should hear back from us {within the next 6 hours}. You can also check for updates about your application by logging into your account. As soon as we complete our checks, we will email you and send you a text message, so thanks for your patience in the meantime.\nIf approved you will just need to come back to the site and click the ‘I accept’ button on your agreement and we will then send the money to your bank within 15 minutes."},
        {19, @"You informed us that you wanted to cancel your credit agreement please contact us on {CS tel. No} to complete this process by making the required repayment."},
        {21, @"One last step to receive your cash.\n\nYour application has been approved! Now you just need to read and accept your new agreement and the loan conditions by clicking the ‘I accept’ button in the agreement below. You will then receive {£loan amount} in your account.\n\nWe’ll then collect {£xx.xx total repayable on due date} from your debit card on {repayment date in format 15th March 2011.}\n\nThanks for using Wonga!"},
	    };

        [Test, AUT(AUT.Uk), JIRA("UK-795", "UK-1614"), Pending("Fails as not covered by the Spec")]
        public void LoanStatusMessageScenario1A()
        // part of L0 journey where the user fill in Personal, Address and Account details and stops the journey.
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

            Assert.IsFalse(mySummaryPage.IsLoanStatusMessageAvailable());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario1B()
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

            Assert.IsFalse(mySummaryPage.IsLoanStatusMessageAvailable());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario2A() { LoanStatusMessage(2, 0); }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario2B() { LoanStatusMessage(2, 1); }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario2C() { LoanStatusMessage(2, 2); }


        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Wrong actual text message. Waiting for code update.")]
        public void LoanStatusMessageScenario3A() { LoanStatusMessage(3, 3); }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Wrong actual text message. Waiting for code update.")]
        public void LoanStatusMessageScenario3B() { LoanStatusMessage(3, 7); }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Wrong actual text message. Waiting for code update.")]
        public void LoanStatusMessageScenario3C() { LoanStatusMessage(3, 9); }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Wrong actual text message. Waiting for code update.")]
        public void LoanStatusMessageScenario3D() { LoanStatusMessage(3, 0, 1); } //0 days passed in 1-day loan

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Wrong actual text message. Waiting for code update.")]
        public void LoanStatusMessageScenario3E() { LoanStatusMessage(3, 0, 7); } //0 days passed in 7-day loan

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Wrong actual text message. Waiting for code update.")]
        public void LoanStatusMessageScenario3F() { LoanStatusMessage(3, 6, 7); } //6 days passed in 7-day loan


        [Test, AUT(AUT.Uk), JIRA("UK-795", "UK-1359")]
        public void LoanStatusMessageScenario4A() { LoanStatusMessage(4, 10); } // payment missed and Next Due Date is today

        [Test, AUT(AUT.Uk), JIRA("UK-795", "UK-1359")]
        public void LoanStatusMessageScenario4B() { LoanStatusMessage(4, 1, 1); } //1 day passed in 1-day loan, // payment missed and Next Due Date is today

        [Test, AUT(AUT.Uk), JIRA("UK-795", "UK-1359")]
        public void LoanStatusMessageScenario4C() { LoanStatusMessage(4, 7, 7); } //7 days passed in 7-day loan, // payment missed and Next Due Date is today

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("TBD: create test for the variant when 3 (=maximum) extensions have been made and it is 1st day after a 7 day loan is taken")]
        public void LoanStatusMessageScenario4D() { LoanStatusMessage(4, 1, 7); }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("TBD: create test for the variant when 3 (=maximum) extensions have been made and it is 10th day after a 10 day loan is taken")]
        public void LoanStatusMessageScenario4E() { LoanStatusMessage(4, 10, 10); }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("TBD: create test for the variant when 2 (< maximum) extensions have been made and it is 6th day after a 10 day loan is taken. The message shold like from scenario 3")]
        public void LoanStatusMessageScenario4F() { LoanStatusMessage(3, 6); }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("TBD: create test for the variant when 1 (< maximum) extensions have been made and it is 1st day after a 10 day loan is taken. The message shold like from scenario 2")]
        public void LoanStatusMessageScenarioG() { LoanStatusMessage(2, 1); } 


        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario5() { LoanStatusMessage(5, 2); }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Wrong actual text message. Waiting for code update.")]
        public void LoanStatusMessageScenario6() { LoanStatusMessage(6, 3); }

        [Test, AUT(AUT.Uk), JIRA("UK-795, UK-1433"), Pending("Fails due to bug UK-1433")]
        public void LoanStatusMessageScenario7() { LoanStatusMessage(7, 10); }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario8() { LoanStatusMessage(8, 10); }


        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario9()
        {
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
            Assert.AreEqual(9, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check Loan Status message
            var expectedLoanMessageText = loanStatusMessages[9];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Hangs")]
        public void LoanStatusMessageScenario10()
        {
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
            Assert.AreEqual(10, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check Loan Status message
            var expectedLoanMessageText = loanStatusMessages[10];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
        } 


        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Wrong actual text message (from scenario 4). Waiting for code update.")]
        public void LoanStatusMessageScenario11() { LoanStatusMessage(11, 14); }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Wrong actual text message (from scenario 4). Waiting for code update.")]
        public void LoanStatusMessageScenario12() { LoanStatusMessage(12, 44); }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Wrong actual text message (from scenario 4). Waiting for code update.")]
        public void LoanStatusMessageScenario13() { LoanStatusMessage(13, 74); }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Awating Repayment Arrangment Functionality.")]
        public void LoanStatusMessageScenario14()
        {
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
            Assert.AreEqual(14, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check Loan Status message
            Assert.IsFalse(mySummaryPage.IsLoanStatusMessageAvailable());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Awating Repayment Arrangment Functionality.")]
        public void LoanStatusMessageScenario15()
        {
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
            Assert.AreEqual(15, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check Loan Status message
            var expectedLoanMessageText = loanStatusMessages[15];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Awating Repayment Arrangment Functionality.")]
        public void LoanStatusMessageScenario16()
        {
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
            setupData.Scenario16Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(16, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check Loan Status message
            var expectedLoanMessageText = loanStatusMessages[16];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
        } 

        [Test, AUT(AUT.Uk), JIRA("UK-795", "UK-1624"), Pending("Fails due to bug UK-1624")]
        public void LoanStatusMessageScenario17A()
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

            var expectedLoanMessageText = loanStatusMessages[17];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage.Replace("{within the next 6 hours}", "within the next 6 hours"); //TBD - replace hardcoded hours with a calculated value
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-795", "UK-1624"), Pending("Fails due to bug UK-1624")]
        public void LoanStatusMessageScenario17B()
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

            var expectedLoanMessageText = loanStatusMessages[17];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage.Replace("{within the next 6 hours}", "within the next 6 hours"); //TBD - replace hardcoded hours with a calculated value
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
        }


        [Test, AUT(AUT.Uk), JIRA("UK-795"), Pending("Waiting for implementation of agreement cancellation process.")]
        public void LoanStatusMessageScenario19() { }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario20() { LoanStatusMessage(20, 10); }

        [Test, AUT(AUT.Uk), JIRA("UK-795", "UK-1735"), Pending("Fails due to bug UK-1735, Waiting for implementation of calculation.")]
        public void LoanStatusMessageScenario21()
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
                .FillCardDetails()
                .WaitForAcceptedPage();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            //{£xx.xx total repayable on due date} from your debit card on {repayment date in format 15th March 2011.}
            var expectedLoanMessageText = loanStatusMessages[21].Replace("{£loan amount}", "£100").Replace("{£xx.xx total repayable on due date}", "£115.91").Replace("{repayment date in format 15th March 2011.}", Date.GetOrdinalDate(DateTime.Today.AddDays(days), "ddd d MMM yyyy"));
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
        }


        //private void LoanStatusMessage(int scenarioId, int daysShift)
        private void LoanStatusMessage(int scenarioId, params int[] days)
        {
            int daysShift = days[0];
            int loanTerm = 10;
            if (days.Length == 2) loanTerm = days[1];

            // Create a customer
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);
            Customer customer;
            if (scenarioId == 20) customer = CustomerBuilder.New().WithEmailAddress(email).WithEmployerStatus(EmploymentStatusEnum.Unemployed.ToString()).WithMiddleName(RiskMask.TESTEmployedMask).Build();
            else customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            // Create a loan
            Application application;
            if (scenarioId < 5) application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();
            else if ((scenarioId >= 5) && (scenarioId <= 7)) application = ApplicationBuilder.New(customer).WithLoanAmount(400).WithLoanTerm(loanTerm).Build();
            // else if (scenarioId == 17) application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Pending).WithLoanTerm(loanTerm).Build();
            else if (scenarioId == 20) application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).WithLoanTerm(loanTerm).Build();
            else application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();


            // Rewind application dates
            ApplicationEntity applicationEntity = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            Drive.Db.RewindApplicationDates(applicationEntity, riskApplication, daysShiftSpan);
            
            if (scenarioId == 8) application = application.RepayOnDueDate(); // Repay a loan

            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            if (scenarioId == 11 || scenarioId == 12 || scenarioId == 13)
            {
                // Send command to create scheduled payment request
                Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = application.Id, RepaymentRequestId = requestId1, });
                Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = application.Id, RepaymentRequestId = requestId2, });
            }

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check Loan Message is displayed/correct
            if ((scenarioId == 8) || (scenarioId == 20))
            {
                Assert.IsFalse(mySummaryPage.IsLoanStatusMessageAvailable());
                return;
            }

            string expectedLoanMessageText = loanStatusMessages[scenarioId];
            if ((scenarioId == 2) || (scenarioId == 5))
            {
                if (application.LoanTerm > 7)
                {
                    var extensionStartDate = applicationEntity.FixedTermLoanApplicationEntity.NextDueDate.Value.AddDays(-7);
                    expectedLoanMessageText = expectedLoanMessageText.Replace("{date extensions available}", Date.GetOrdinalDate(extensionStartDate, "ddd d MMM yyyy"));
                }
                else // if loan period is less than 7 days
                {
                    expectedLoanMessageText = loanStatusMessages[3];  // can extend right now  
                }
            }

            if (scenarioId == 4) expectedLoanMessageText = expectedLoanMessageText.Replace("{total to repay 300.00}", application.GetDueDateBalance().ToString("#.##")).Replace("{promise date}", Date.GetOrdinalDate(applicationEntity.FixedTermLoanApplicationEntity.NextDueDate.Value, "ddd d MMM yyyy"));

            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
        }
    }
}
