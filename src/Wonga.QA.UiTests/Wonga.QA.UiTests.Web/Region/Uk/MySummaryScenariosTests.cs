using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Testing.Attributes;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    /// <summary>
    /// Check content of My Summary page against different scenarios
    /// </summary>
    [Parallelizable(TestScope.All), AUT(AUT.Uk), JIRA("UK-785", "UK-788", "UK-795", "UKWEB-151", "UKWEB-985")]
    public class MySummaryScenariosTests : UiTest
    {
        decimal _amountDueAt60InArrears = 0.00m;
        private string _emailScenario14;

        #region Dictionaries
        Dictionary<int, string> introTexts = new Dictionary<int, string> 
	    {
	    {1, "Hi {first name}. Your current trust rating means you can apply for up to �{500} below."},
        {2, "Hi {first name}. You currently have up to �{245.00} of available credit which you can request at any time."},
        {3, "Hi {first name}. You currently have up to �{245.00} of available credit which you can request at any time."},
        {4, "Hi {first name}. You currently have up to �{245.00} of available credit which you can request at any time."},
        {5, "Hi {first name}. You don't currently have any available credit and you promised to repay �{425.00} on {Friday 13 Feb 2011}."},
        {6, "Hi {first name}. You don't currently have any available credit and you promised to repay �{425.00} on {Friday 13 Feb 2011}."},
        {7, "Hi {first name}. You don't currently have any available credit and you promised to repay �{425.00} on {Friday 13 Feb 2011}."},
        {8, "Hi {first name}. We collected your full payment of �{300} today, as promised.Many thanks for repaying our trust in you.You can now request your available credit up to your current Wonga trust rating of �{500}, at anytime. Thanks for using Wonga and we hope we can help again in the future."},
        {9, "Hi {first name}. Your promised repayment of �{456.34}, due first thing today, was declined by your bank."},
        {10, "Hi {first name}. Your repayment of �{456.34}, promised on {date}, was declined by your bank and is now overdue."},
        {11, "Hi {first name}. Your account is now {26} days in arrears."},
        {12, "Hi {first name}. We are disappointed that your account remains overdue and you are now {46} days in arrears."},
        {13, "Hi {first name}. We are disappointed that your account remains overdue and you are now {61} days in arrears."},
        {14, "Hi {first name}."},
        {15, "Hi {first name}."},
        {16, "Hi {first name}."},
        {17, "Hi {first name}."},
        {19, "Hi {first name}."},
        {20, "Hi {first name}. As a new customer you can apply for up to �{400} below."},
        {21, "Hi {first name}."}
	    };

        Dictionary<int, string> tagCloudTexts = new Dictionary<int, string> 
	    {
         {1, ""},
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
         {14, ""},
         {15, ""},
         {16, ""},
         {17, ""},
         {19, ""},
         {20, ""},
         {21, ""},
	    };

        Dictionary<int, string> loanStatusMessages = new Dictionary<int, string> 
	    {
        {1, ""},
	    {2, "If you would like to change your repayment date it's too early to do it just yet, but you can request a new one from {date extensions available}. You can set a handy reminder to do that below. Please bear in mind that you will need to pay any interest and fees up to that point, in order for a new date to be approved."},
        {3, "If you would like to change your promised repayment date you can do so here. Please note you can only extend your promise date a maximum of three times and will need to pay any interest and fees up to that point each time you extend, in order to have any request approved."},
        {4, "Please also remember you have promised to repay �{total to repay 300.00} on {promise date}, when you simply need to ensure the funds are available in the bank account linked to your primary debit card. Changing your promise date isn't possible at this point, so we look forward to collecting your payment and then being of service again in the future."},
        {5, "If you would like to change your repayment date it's too early to do it just yet, but you can request a new one from {date extensions available}. You can set a handy reminder to do it below. Please bear in mind that you will need to pay any interest and fees up to that point, in order to have your request for a change of promise date approved."},
        {6, "If you would like to change your promised repayment date you can do so here. Please note you can only extend your promise date a maximum of three times and will need to pay any interest and fees up to that point each time you extend, in order to have any request approved."},
        {7, "Changing your promise date isn't possible at this point, so we look forward to collecting your full payment and then being of service again in the future. Thanks for using Wonga!"},
        {9, "We understand genuine mistakes happen so we hope you can make this payment today and save yourself further costs. If the balance isn't cleared by 5pm today, however, you will incur a missed payment fee of �20, which is the last thing we want to happen! Please click repay now to settle your balance. You can add a new debit card if you need to. If you are unable to pay in full today, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {10, "You have unfortunately incurred a missed payment fee of �20 and interest continues to accrue. Please click repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. If you are unable to pay in full today, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {11, "Please act now to avoid incurring further interest, which continues to accrue. Please click repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of four months. If this doesn't work for you, you should call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {12, "Please act now to avoid incurring further interest, which continues to accrue, and potential negative entries on your credit file. Please click repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of six months. If this doesn't work for you, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {13, "Please take action today to avoid incurring further interest, which continues to accrue, and potential negative entries on your credit file. Click Repay now to settle your balance and bring your account back into line. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of six months. If this doesn't work for you, please call our friendly collections team straight away on 0844 842 9109. If you choose not to deal with this matter immediately, we may need to take more formal steps to recover the balance owed."},
        {14, "You have an active repayment plan\r\nTo make an early repayment or discuss your plan, please contact our customer care team on 0844 842 9109."},
        {15, "You have an active repayment plan\r\nTo make an early repayment or discuss your plan, please contact our customer care team on 0844 842 9109."},
        {16, "You have an active repayment plan\r\nTo make an early repayment or discuss your plan, please contact our customer care team on 0844 842 9109."},
        {17, "Your application is in the final stages of our approval process. We hate to keep you waiting, but, on this rare occasion, we need to check a few more details. There's no need to contact us or do anything and you should hear back from us {within the next 6 hours}. You can also check for updates about your application by logging into your account. As soon as we complete our checks, we will email you and send you a text message, so thanks for your patience in the meantime.\nIf approved you will just need to come back to the site and click the �I accept� button on your agreement and we will then send the money to your bank within 15 minutes."},
        {19, "You informed us that you wanted to cancel your credit agreement please contact us on {CS tel. No} to complete this process by making the required repayment."},
        {21, "One last step to receive your cash.\n\nYour application has been approved! Now you just need to read and accept your new agreement and the loan conditions by clicking the �I accept� button in the agreement below. You will then receive {�loan amount} in your account.\n\nWe�ll then collect {�xx.xx total repayable on due date} from your debit card on {repayment date in format 15th March 2011.}\n\nThanks for using Wonga!"},
	    };

        Dictionary<int, string> PromiseSummaryTexts = new Dictionary<int, string> 
	    {
	    {1, ""},
        {2, "I promised to pay {�245} in {10} days ({10th May 2012})"},
        {3, "I promised to pay {�245} in {10} days ({10th May 2012})"},
        {4, "I promised to pay {�245} in {10} days ({10th May 2012})"},
        {5, "I promised to pay {�245} in {10} days ({10th May 2012})"},
        {6, "I promised to pay {�245} in {10} days ({10th May 2012})"},
        {7, "I promised to pay {�245} in {10} days ({10th May 2012})"},
        {8, ""},
        {9, "I promised to pay {�456.34} today"},
        {10, "I owe {�456.34} today"},
        {11, "I owe {�456.34} today"},
        {12, "I owe {�456.34} today"},
        {13, "I owe {�456.34} today"},
        {14, ""},
        {15, ""},
        {16, ""},
        {17, ""},
        {19, ""},
        {20, ""},
        {21, ""}
	    };

        Dictionary<int, string> sliderType = new Dictionary<int, string> 
	    {
	    {1, "full"},
        {2, "half"},
        {3, "half"},
        {4, "half"},
        {5, "no"},
        {6, "no"},
        {7, "no"},
        {8, "full"},
        {9, "no"},
        {10, "no"},
        {11, "no"},
        {12, "no"},
        {13, "no"},
        {14, "no"},
        {15, "no"},
        {16, "no"},
        {17, "no"},
        {19, "no"},
        {20, "full"},
        {21, "no"}
	    };

        #endregion

        // Check the my Summary page after we click My Summary buton
        [Test, JIRA("UKWEB-953"), Owner(Owner.StanDesyatnikov)]
        public void ClickMySummaryButton()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).Build();

            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
        }

        // No live drawdowns: L0 journey
        [Test, JIRA("UKWEB-419"), MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Pending("UKWEB-419: Scenario 17 instead of scenario 1, when user drops off in L0 Bank Account page")]
        public void MySummaryScenario1L0()
        {
            int scenarioId = 1;
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            string firstName = Get.RandomString(3, 10);
            Console.WriteLine("ScenarioId={0}; Email={1}", scenarioId, email);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email)
                .WithFirstName(firstName)
                .WithAmount(loanAmount).WithDuration(days);
            var aPage = journey.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");

            // Check the actual text
            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedIntroText = introTexts[1];
            expectedIntroText = expectedIntroText.Replace("{first name}", firstName).Replace("{500}", "400.00");

            Assert.AreEqual(expectedIntroText, actuallntroText);
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");
            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
            Assert.IsFalse(mySummaryPage.IsLoanStatusMessageAvailable());

            CheckSliders(scenarioId, mySummaryPage);
        }

        // No live drawdowns: Ln journey
        [Test, MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        public void MySummaryScenario1Ln()
        {
            int scenarioId = 1;
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

            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedIntroText = introTexts[1];
            expectedIntroText = expectedIntroText.Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]).Replace("{500}", "400.00");

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");
            Assert.AreEqual(expectedIntroText, actuallntroText, "Intro text is wrong");
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "I Promise Summary should not be availble");
            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable(), "Tag Cloud should not be availble");
            Assert.IsFalse(mySummaryPage.IsLoanStatusMessageAvailable(), "Loan Status Message should not be availble");
            CheckSliders(scenarioId, mySummaryPage);
        }

        // One live drawdown -can request credit, too early to extend
        [Test, JIRA("UK-1737"), MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Row(2, 0)]
        [Row(2, 1)]
        [Row(2, 2)]
        public void MySummaryScenario02(int scenarioId, int dayShift) { MySummaryScenarios(scenarioId, dayShift); }

        // One live drawdown -can request credit, can extend
        [Test, MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Row(3, 3, 10)]
        [Row(3, 7, 10)]
        [Row(3, 8, 10)]
        public void MySummaryScenario03(int scenarioId, int dasyShift, int loanTerm) { MySummaryScenarios(scenarioId, dasyShift, loanTerm); }

        // One live drawdown -can request credit, can't  extend (max. exceeded)
        [Test, MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        public void MySummaryScenario04()
        {
            string email = Get.RandomEmail();
            const int loanTerm = 2;
            const int loanAmount = 100;
            const int scenarioId = 4;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
            var daysShift = 0;
            for (int i = 1; i <= 3; i++)
            {
                mySummaryPage.ChangePromiseDateButtonClick();
                var requestPage = new ExtensionRequestPage(this.Client);

                //Runs assertions internally
                daysShift = 1;
                requestPage.SetExtendDays(daysShift.ToString("#"));

                //Branch point - Add Cv2 for each path and proceed
                requestPage.setSecurityCode("123");
                requestPage.SubmitButtonClick();

                var extensionProcessingPage = new ExtensionProcessingPage(this.Client);

                var agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;
                agreementPage.Accept();

                var dealDonePage = new ExtensionDealDonePage(this.Client);

                Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "/my-account");
                mySummaryPage = new MySummaryPage(this.Client);

                CheckPromiseSummaryText(scenarioId, mySummaryPage, application);
            }

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");

            CheckIntroText(scenarioId, mySummaryPage, customer, application);
            CheckLoanStatusText(scenarioId, mySummaryPage, customer, application);
            CheckTagCloud(scenarioId, mySummaryPage);
            CheckSliders(scenarioId, mySummaryPage);
            CheckPromiseSummaryText(scenarioId, mySummaryPage, application);

            try
            {
                mySummaryPage.ChangePromiseDateButtonClick();
            }
            catch (Exception){}
            finally
            {
                mySummaryPage = new MySummaryPage(this.Client);
            }
        }

        // One live drawdown -can't request credit, too early to extend
        [Test, JIRA("UK-788", "UK-1909"), MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Row(5, 0)] //0 days passed in 10-day loan
        [Row(5, 2)] //0 days passed in 10-day loan
        public void MySummaryScenario05(int scenarioId, int dasyShift) { MySummaryScenarios(scenarioId, dasyShift); }

        // One live drawdown -can't request credit (Too near to due date), can extend
        [Test, MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Row(6, 3, 10)] //3 days passed in 10-day loan
        [Row(6, 6, 7)]  //6 days passed in 7-day loan
        public void MySummaryScenario06(int scenarioId, int dasyShift, int loanTerm) { MySummaryScenarios(scenarioId, dasyShift, loanTerm); }

        // One live drawdown-can't request credit, can't extend (too late or max. exceeded
        [Test, MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Row(7, 10)]
        [Pending("UKWEB-1135: Wrong 'I Promise' text on My Summary page for scenario 7")]
        public void MySummaryScenario07(int scenarioId, int dasyShift) { MySummaryScenarios(scenarioId, dasyShift); }

        // On promise date after live loan closed following successful repayment event
        [Test, MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Row(8, 10), JIRA("UKWEB-483")] 
        [Pending("UKWEB-483: Waiting for implementation of GetRepayLoanStatus")]
        public void MySummaryScenario08(int scenarioId, int dasyShift) { MySummaryScenarios(scenarioId, dasyShift); }

        // Live loan on promise date (after failed ping until missed payment fee is applied
        [Test, MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        public void MySummaryScenario09()
        {
            const int scenarioId = 9;
            string email = Get.RandomEmail();
            Console.WriteLine("ScenarioId={0}; Email={1}", scenarioId, email);

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
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId returned by API");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");

            response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = appId });
            var expectedNextDueDateRepay = Convert.ToDecimal(response.Values["BalanceNextDueDate"].Single());

            var expectedIntroText = introTexts[scenarioId].Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]);
            expectedIntroText = expectedIntroText.Replace("Your promised repayment of �{456.34}", "Your promised repayment of �" + expectedNextDueDateRepay.ToString("#.00"));
            string actualIntroText = mySummaryPage.GetIntroText;
            //Assert.AreEqual(expectedIntroText, actualIntroText);

            string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);

            var expectedLoanMessageText = loanStatusMessages[9];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);

            CheckSliders(scenarioId, mySummaryPage);
            CheckPromiseSummaryText(scenarioId, mySummaryPage, customer.GetApplication());

           // ChangeWantToRepayBox(customer, customer.GetApplication());
        }

        // Live loan  promise date (after missed payment  fee applied < 3 days in arrears.)
        [Test, MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        public void MySummaryScenario10()
        {
            const int scenarioId = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("ScenarioId={0}; Email={1}", scenarioId, email);

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
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId returned by API");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
            
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");

            response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = appId });
            var expectedBalanceToday = String.Format("{0:0.00}", Convert.ToDecimal(response.Values["BalanceToday"].Single()));
            var expectedNextDueDate = Date.GetOrdinalDate(Convert.ToDateTime(response.Values["NextDueDate"].Single()), "ddd d MMM yyyy");

            //Your repayment of �{456.34}, promised on {date}
            var expectedIntroText = introTexts[scenarioId].Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]);
            expectedIntroText = expectedIntroText.Replace("Your repayment of �{456.34}", "Your repayment of �" + expectedBalanceToday);
            expectedIntroText = expectedIntroText.Replace("promised on {date}", "promised on " + expectedNextDueDate);
            string actualIntroText = mySummaryPage.GetIntroText;
            Assert.AreEqual(expectedIntroText, actualIntroText);

            string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);

            // Check Loan Status message
            var expectedLoanMessageText = loanStatusMessages[10];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);

            string expectedPromiseSummaryText = PromiseSummaryTexts[scenarioId].Replace("{�456.34}", "�" + expectedBalanceToday);
            Assert.AreEqual(expectedPromiseSummaryText, mySummaryPage.GetPromiseSummaryText);

            CheckSliders(scenarioId, mySummaryPage);

           // ChangeWantToRepayBox(customer, customer.GetApplication());
        }

        // 3+ days in arrears
        [Test, MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Row(11, 3)]
        [Row(11, 4)]
        [Row(11, 30)]
        public void MySummaryScenario11(int scenarioId, int dasyShift) { MySummaryScenarios(scenarioId, dasyShift); }

        // 31+days in arrears
        [Test, JIRA("UK-1954"), MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Row(12, 31)]
        [Row(12, 32)]
        [Row(12, 60)]
        public void MySummaryScenario12(int scenarioId, int dasyShift) { MySummaryScenarios(scenarioId, dasyShift); }

        // 61+ days in arrears
        [Test, JIRA("UK-1966"), MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Pending("UKWEB-1169: Amount Due continues increasing after 60 days in arrears")]
        [Row(13, 61)]
        [Row(13, 62)]
        [Row(13, 100)]
        [Row(13, 1000)]
        public void MySummaryScenario13(int scenarioId, int dasyShift) { MySummaryScenarios(scenarioId, dasyShift); }

        // In arrears -In repayment plan
        [Test, JIRA("UKWEB-1083"), MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Pending("UKWEB-1128: Error is displayed on My Summary page")]
        public void MySummaryScenario14()
        {
            var scenarioId = 14;
            string email = Get.RandomEmail();
            _emailScenario14 = email;
            Console.WriteLine("ScenarioId={0}; Email={1}", scenarioId, email);
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
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId returned by API");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");

            string expectedIntroText = introTexts[scenarioId].Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]);
            string actualIntroText = mySummaryPage.GetIntroText;
            Assert.AreEqual(expectedIntroText, actualIntroText);

            /*string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);*/

            //Assert.IsFalse(mySummaryPage.IsLoanStatusMessageAvailable());

            var expectedLoanMessageText = loanStatusMessages[scenarioId];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);

            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");

            CheckSliders(scenarioId, mySummaryPage);
        }

        [Test, MultipleAsserts, JIRA("UKWEB-1178"), Owner(Owner.StanDesyatnikov), DependsOn("MySummaryScenario14")]
        [Pending("UKWEB-1178: Scenario 14 users state is not being maintained")]
        public void CheckMySummaryScenario14AfterRelogin()
        {
            int scenarioId = 14;

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(_emailScenario14);
            //var mySummaryPage = loginPage.LoginAsIgnoreError(_emailScenario14);

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Scenario is wrong after page refresh.");

            /*Validator validator = new ValidatorBuilder().Default(Client).WithoutErrorsCheck().Build();
            Client.Driver.Navigate().GoToUrl(Client.Home().Url);
            var homePage = Client.Home();
            homePage.ClickWelcomeMessageClickHereLink();
            //homePage = new HomePage(this.Client);
            var loginPage = Client.Login();
            //var mySummaryPage = loginPage.LoginAs(email);
            mySummaryPage = loginPage.LoginAsIgnoreError(email);


            //Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
            //Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");
            //CheckSliders(scenarioId, mySummaryPage);
            */
            Console.WriteLine("Page refreshed.");
        }

        // In arrears -In repayment plan - missed payment (within grace period)
        [Test, JIRA("UKWEB-1083"), MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Pending("UKWEB-1128: Error is displayed on My Summary page")]
        public void MySummaryScenario15()
        {
            const int scenarioId = 15;
            string email = Get.RandomEmail();
            Console.WriteLine("ScenarioId={0}; Email={1}", scenarioId, email);
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
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId returned by API");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");

            string expectedIntroText = introTexts[scenarioId].Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]);
            string actualIntroText = mySummaryPage.GetIntroText;
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");
            Assert.AreEqual(expectedIntroText, actualIntroText);

            /*string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);
            */
            var expectedLoanMessageText = loanStatusMessages[scenarioId];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");
            CheckSliders(scenarioId, mySummaryPage);
        }

        // In arrears - In repayment plan � broken repayment arrangemnt
        [Test, JIRA("UKWEB-1083"), MultipleAsserts]
        [Pending("UKWEB-1128: Error is displayed on My Summary page")]
        public void MySummaryScenario16()
        {
            const int scenarioId = 16;
            string email = Get.RandomEmail();
            Console.WriteLine("ScenarioId={0}; Email={1}", scenarioId, email);
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
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId returned by API");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
            
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");

            string expectedIntroText = introTexts[scenarioId].Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]);
            string actualIntroText = mySummaryPage.GetIntroText;
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");
            Assert.AreEqual(expectedIntroText, actualIntroText);

            /*string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);
            */
            var expectedLoanMessageText = loanStatusMessages[scenarioId];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");

            CheckSliders(scenarioId, mySummaryPage);
        }

        // No live drawdowns
        [Test, JIRA("UK-1624"), MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Pending("UK-1624: Waiting for implementation of Referrals and API implementation of the hours to decision")]        
        public void MySummaryScenario17A()
        {
            var scenarioId = 17;
            string firstName = Get.RandomString(3, 10);
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("ScenarioId={0}; Email={1}", scenarioId, email);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email)
                .WithFirstName(firstName)
                .WithAmount(loanAmount).WithDuration(days);
            var aPage = journey.Teleport<PersonalDebitCardPage>() as PersonalDebitCardPage;

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");
            
            // Check the actual text
            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedIntroText = introTexts[17];
            expectedIntroText = expectedIntroText.Replace("{first name}", firstName);
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");
            Assert.AreEqual(expectedIntroText, actuallntroText);

            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");
            var expectedLoanMessageText = loanStatusMessages[17];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage.Replace("{within the next 6 hours}", "within the next 6 hours"); //TBD - replace hardcoded hours with a calculated value
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");
            CheckSliders(scenarioId, mySummaryPage);
        }

        // No live drawdowns
        [Test, JIRA("UK-1624"), MultipleAsserts] 
        [Owner(Owner.StanDesyatnikov)]
        [Pending("UK-1624: Waiting for implementation of Referrals and API implementation of the hours to decision")]
        public void MySummaryScenario17B()
        {
            var scenarioId = 17;
            string firstName = Get.RandomString(3, 10);
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("ScenarioId={0}; Email={1}", scenarioId, email);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email)
                .WithFirstName(firstName)
                .WithAmount(loanAmount).WithDuration(days);
            var aPage = journey.Teleport<AcceptedPage>() as AcceptedPage;

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
            
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");

            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedIntroText = introTexts[17];
            expectedIntroText = expectedIntroText.Replace("{first name}", firstName);
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");
            Assert.AreEqual(expectedIntroText, actuallntroText);

            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");
            var expectedLoanMessageText = loanStatusMessages[17];
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage.Replace("{within the next 6 hours}", "within the next 6 hours"); //TBD - replace hardcoded hours with a calculated value
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");
            CheckSliders(scenarioId, mySummaryPage);
        }

        // Agreement being cancelled
        [Test, MultipleAsserts] 
        [Owner(Owner.StanDesyatnikov)]
        [Pending("Waiting for implementation of agreement cancellation process.")]
        public void MySummaryScenario19() { MySummaryScenarios(19, 0); }

        // Customer has never had a loan but has applied and been declined therefore has an account
        [Test, MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        public void MySummaryScenario20() { MySummaryScenarios(20, 1); }

        // Needs to accept agreement to complete application
        [Test, MultipleAsserts, JIRA("UK-1735")]
        [Pending("UK-1735: Waiting for implementation of calculation")]
        [Owner(Owner.StanDesyatnikov)]
        public void MySummaryScenario21()
        {
            var scenarioId = 21;
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            string firstName = Get.RandomString(3, 10);
            Console.WriteLine("ScenarioId={0}; Email={1}", scenarioId, email);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email)
                .WithFirstName(firstName)
                .WithAmount(loanAmount).WithDuration(days);
            var aPage = journey.Teleport<PersonalDebitCardPage>() as PersonalDebitCardPage;

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");

            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedIntroText = introTexts[scenarioId];
            expectedIntroText = expectedIntroText.Replace("{first name}", firstName);
            Assert.AreEqual(expectedIntroText, actuallntroText);
   
            Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");
            //{�xx.xx total repayable on due date} from your debit card on {repayment date in format 15th March 2011.}
            var expectedLoanMessageText = loanStatusMessages[scenarioId].Replace("{�loan amount}", "�100").Replace("{�xx.xx total repayable on due date}", "�115.91").Replace("{repayment date in format 15th March 2011.}", Date.GetOrdinalDate(DateTime.Today.AddDays(days), "ddd d MMM yyyy"));
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
            Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "PromiseSummary should not be availble");
            CheckSliders(scenarioId, mySummaryPage);
        }

        # region Functions

        //private void MySummaryScenarios(int scenarioId, params int[] days)
        private void MySummaryScenarios(int scenarioId, int daysShift, int loanTerm=10)
        {
            var expectedDueDateBalance = 0.00M;
            var expectedAmountMax = "0";

            //int daysShift = days[0];
            //int loanTerm = 10;
            //if (days.Length == 2) loanTerm = days[1];

            string email = Get.RandomEmail();
            Customer customer = CreateCustomerForScenario(scenarioId, email);
            
            Application application = CreateApplicationForScenario(scenarioId, customer, loanTerm);

            RewindApplicationDates(loanTerm, daysShift, application);
            
            if (scenarioId == 8)
            {
                expectedAmountMax = "400.00";
            }

            if (scenarioId == 20)
            {
                expectedAmountMax = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id }).Values["AvailableCredit"].Single();
                expectedAmountMax = String.Format("{0:0.00}", Convert.ToDecimal(expectedAmountMax));
            }

            expectedDueDateBalance = GetExpectedDueDateBalance(application, scenarioId);

            RepayOnDueDate(scenarioId, application);
            
            SchedulePayment(scenarioId, application);

            var mySummaryPage = Client.Login().LoginAs(email);

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong ScenarioID.");

            CheckTagCloud(scenarioId, mySummaryPage);
            CheckIntroText(scenarioId, mySummaryPage, customer, application, expectedAmountMax, expectedDueDateBalance);
            CheckLoanStatusText(scenarioId, mySummaryPage, customer, application);
            CheckPromiseSummaryText(scenarioId, mySummaryPage, application);
            CheckSliders(scenarioId, mySummaryPage);

            //if (mySummaryPage.GetTagCloud.IndexOf("Repay") > 0) ChangeWantToRepayBox(customer, application);
        }

        // Create a customer for a scenario
        private Customer CreateCustomerForScenario(int scenarioId, string email)
        {
            // Create a customer
            Console.WriteLine("ScenarioId={0}; Email={1}", scenarioId, email);
            Customer customer;
            
            if (scenarioId == 20) 
                customer = CustomerBuilder.New().WithEmailAddress(email).WithEmployerStatus(EmploymentStatusEnum.Unemployed.ToString()).WithMiddleName(RiskMask.TESTEmployedMask).Build();
            else 
                customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            return customer;
        }

        // Create an application for a scenario
        private Application CreateApplicationForScenario(int scenarioId, Customer customer, int loanTerm)
        {

            int loanAmount = 100;
            ApplicationDecisionStatus applicationDecisionStatus = ApplicationDecisionStatus.Accepted;

            if ((scenarioId >= 5) && (scenarioId <= 7)) loanAmount = 400;
            else if (scenarioId == 20) applicationDecisionStatus = ApplicationDecisionStatus.Declined;                
            
            Application application = ApplicationBuilder.New(customer)
                .WithLoanAmount(loanAmount)
                .WithLoanTerm(loanTerm)
                .WithExpectedDecision(applicationDecisionStatus).Build();

            return application;
        }

        // Rewind application dates
        private void RewindApplicationDates(int loanTerm, int daysShift, Application application)
        {
            if (daysShift != 0)
            {
                if (daysShift > 60)
                {
                    TimeSpan daysShiftSpan = TimeSpan.FromDays(loanTerm + 60);
                    ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);
                    _amountDueAt60InArrears = application.GetBalanceToday();
                    daysShiftSpan = TimeSpan.FromDays(daysShift - 60);
                    ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);
                }
                else if (daysShift > loanTerm)
                {
                    TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift + loanTerm);
                    ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);
                }
                else
                {
                    TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
                    ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);
                }
            }
        }

        // Schedule payment
        private void SchedulePayment(int scenarioId, Application application)
        {
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            if (scenarioId == 11 || scenarioId == 12 || scenarioId == 13)
            {
                // Send command to create scheduled payment request
                Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequest { ApplicationId = application.Id, RepaymentRequestId = requestId1, });
                Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequest() { ApplicationId = application.Id, RepaymentRequestId = requestId2, });
            }
        }

        // Repay a loan on Due Date
        private void RepayOnDueDate(int scenarioId, Application application)
        {
            if (scenarioId == 8) application = application.RepayOnDueDate(); 
        }

        // Check Tag Cloud on My Summary page
        private void CheckTagCloud(int scenarioId, MySummaryPage mySummaryPage)
        {
            // Check Tag Cloud is displayed/correct
            if ((scenarioId == 8) || (scenarioId == 20))
            {
                Assert.IsFalse(mySummaryPage.IsTagCloudAvailable());
                return;
            }

            string expectedTagCloudText = tagCloudTexts[scenarioId];
            string actualTagCloudText = mySummaryPage.GetTagCloud;

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId), "Back-End returned wrong Scenario.");
            Assert.AreEqual(expectedTagCloudText, actualTagCloudText);

            if (scenarioId == 4)
            {
                Assert.AreEqual("Sorry this is not possible, either because you have done this 3 times before or because it is too late.", mySummaryPage.GetChangePromiseDateButton);
            }
        }

        // Check Intro Text on My Summary page
        private void CheckIntroText(int scenarioId, MySummaryPage mySummaryPage, Customer customer, Application application, string expectedAmountMax = "", decimal expectedDueDateBalance = 0)
        {
            // Check the actual text
            string expectedIntroText = introTexts[scenarioId].Replace("{first name}",
                                                                      customer.GetCustomerFullName().Split(' ')[0]);

            var response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = application.Id });
            var expectedAvailableCredit = Convert.ToDecimal(response.Values["AvailableCredit"].Single());
            expectedIntroText = expectedIntroText.Replace("You currently have up to �{245.00}", "You currently have up to �" + String.Format("{0:0.00}", expectedAvailableCredit));


            var expectedNextDueDateRepay = Convert.ToDecimal(response.Values["BalanceNextDueDate"].Single());
            var dExpectedNextDueDate = Convert.ToDateTime(response.Values["NextDueDate"].Single());
            var expectedNextDueDate = Date.GetOrdinalDate(dExpectedNextDueDate, "ddd d MMM yyyy");
            TimeSpan daysInArrears = DateTime.Today - dExpectedNextDueDate;


            expectedIntroText = expectedIntroText.Replace("you promised to repay �{425.00}", "you promised to repay �" + String.Format("{0:0.00}", expectedNextDueDateRepay));
            expectedIntroText = expectedIntroText.Replace("on {Friday 13 Feb 2011}", "on " + expectedNextDueDate);
            expectedIntroText = expectedIntroText.Replace("We collected your full payment of �{300} today", "We collected your full payment of �" + expectedDueDateBalance + " today");
            expectedIntroText = expectedIntroText.Replace("your current Wonga trust rating of �{500}", "your current Wonga trust rating of �" + expectedAmountMax);
            expectedIntroText = expectedIntroText.Replace("Your account is now {26}", "Your account is now " + daysInArrears.Days.ToString("#"));
            expectedIntroText = expectedIntroText.Replace("you are now {46}", "you are now " + daysInArrears.Days.ToString("#"));
            expectedIntroText = expectedIntroText.Replace("you are now {61}", "you are now " + daysInArrears.Days.ToString("#"));
            expectedIntroText = expectedIntroText.Replace("you can apply for up to �{400}", "you can apply for up to �" + expectedAmountMax);

            string actuallntroText = mySummaryPage.GetIntroText;
            Assert.AreEqual(expectedIntroText, actuallntroText);
        }

        // Check Loan Status Text on My Summary page
        private void CheckLoanStatusText(int scenarioId, MySummaryPage mySummaryPage, Customer customer, Application application)
        {
            // Check Loan Message is displayed/correct
            if ((scenarioId == 8) || (scenarioId == 20))
            {
                Assert.IsFalse(mySummaryPage.IsLoanStatusMessageAvailable());
                return;
            }

            string expectedLoanMessageText = loanStatusMessages[scenarioId];
            
            var paymentsAppsTab = Drive.Data.Payments.Db.Applications;
            dynamic applicationEntity = paymentsAppsTab.FindAll(paymentsAppsTab.ExternalId == application.Id).Single();
            
            if ((scenarioId == 2) || (scenarioId == 5))
            {
                if (application.LoanTerm > 7)
                {
                    ApiResponse response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = application.Id });
                    var extensionStartDate = Convert.ToDateTime(response.Values["NextDueDate"].Single()).AddDays(-7);

                    //var extensionStartDate = applicationEntity.FixedTermLoanApplicationEntity.NextDueDate.Value.AddDays(-7);
                    expectedLoanMessageText = expectedLoanMessageText.Replace("{date extensions available}", Date.GetOrdinalDate(extensionStartDate, "ddd d MMM yyyy"));
                }
                else // if loan period is less than 7 days
                {
                    expectedLoanMessageText = loanStatusMessages[3];  // can extend right now  
                }
            }

            if (scenarioId == 4)
            {
                ApiResponse response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = application.Id });
                DateTime nextDueDay = Convert.ToDateTime(response.Values["NextDueDate"].Single());
                expectedLoanMessageText = expectedLoanMessageText.Replace("{total to repay 300.00}", application.GetDueDateBalance().ToString("#.##")).Replace("{promise date}", Date.GetOrdinalDate(nextDueDay, "ddd d MMM yyyy"));
            }

            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
        }
        
        // Check Sliders on My Summary page
        private void CheckSliders(int scenarioId, MySummaryPage mySummaryPage)
        {
            if (sliderType[scenarioId] == "full")
            {
                Assert.IsNotNull(mySummaryPage.Sliders, "Sliders should be available");
                Assert.IsNotEmpty(mySummaryPage.Sliders.HowLong, "Duration slider should be available");
                Assert.IsNotEmpty(mySummaryPage.Sliders.HowMuch, "Amount slider should be available");
            }
            else if (sliderType[scenarioId] == "half")
            {
                Assert.IsNotNull(mySummaryPage.TopupSliders, "Sliders should be  available");
                Assert.IsNotEmpty(mySummaryPage.TopupSliders.HowMuch, "Amount slider should be available");
            }
            else if (sliderType[scenarioId] == "no")
            {
                Assert.IsNull(mySummaryPage.Sliders, "Sliders should not be available");
            }
        }

        private decimal GetExpectedDueDateBalance(Application application, int scenarioId)
        {
            decimal expectedDueDateBalance = 0;

            if ((scenarioId == 11) || (scenarioId == 12))
            {
                expectedDueDateBalance = application.GetBalanceToday();
            }
            else if (scenarioId == 13)
            {
                expectedDueDateBalance = _amountDueAt60InArrears;
            }
            else
            {
                expectedDueDateBalance = application.GetDueDateBalance();
            }

            return expectedDueDateBalance;
        }

        // Check Promise Summary Text on My Summary page
        private void CheckPromiseSummaryText(int scenarioId, MySummaryPage mySummaryPage, Application application)
        {
            
            var expectedDueDateBalance = GetExpectedDueDateBalance(application, scenarioId);
            var expectedDueDate = application.GetNextDueDate();
            var expectedDaysTillDueDate = expectedDueDate - DateTime.Today.Date; 

            if (PromiseSummaryTexts[scenarioId].Length == 0)
            {
                Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "Promise Summary should not be available");
            }
            else
            {
                string expectedPromiseSummaryText = PromiseSummaryTexts[scenarioId]
                                                    .Replace("{�245}", "�" + String.Format("{0:0.00}", expectedDueDateBalance))
                                                    .Replace("in {10}", "in " + expectedDaysTillDueDate.Days.ToString("#"))
                                                    .Replace("{10th May 2012}", Date.GetOrdinalDate(expectedDueDate, "ddd d MMM yyyy"))
                                                    .Replace("{�456.34}", "�" + String.Format("{0:0.00}", expectedDueDateBalance));
                if ((scenarioId == 7) && (expectedDueDate.Equals(DateTime.Today)))
                {
                    expectedPromiseSummaryText = "I promised to pay {�245} today ({10th May 2012})"
                                                  .Replace("{�245}", "�" + String.Format("{0:0.00}", expectedDueDateBalance))
                                                  .Replace("{10th May 2012}", Date.GetOrdinalDate(expectedDueDate, "ddd d MMM yyyy"));
                }
                Assert.AreEqual(expectedPromiseSummaryText, mySummaryPage.GetPromiseSummaryText);
            }

        }

        // Click Repay in Tag Cloud and check default values on Repay page
        private void ChangeWantToRepayBox(Customer customer, Application application)
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
                sActualOweToday = requestPage.OweToday.TrimStart('�');
                Assert.AreEqual(sExpectedOweToday, sActualOweToday, "Currently Owe is wrong.");

                // Remainder to repay = Amount Owed - Repay Amount
                var expectedReminderToRepay = expectedOweToday - amountToRepay;
                string sExpectedReminderToRepay = String.Format("{0:0.00}", expectedReminderToRepay);
                string sActualReminderToRepay = requestPage.RemainderAmount.TrimStart('�');
                //Assert.AreEqual(sExpectedReminderToRepay, sActualReminderToRepay, "Reminder Amount is wrong.");

                // Repay Total in the Read Me message
                sExpectedWantToRepay = String.Format("{0:0.00}", amountToRepay);
                string sActualRepayTotal = requestPage.RepayTotal.TrimStart('�');
                //Assert.AreEqual(sExpectedWantToRepay, sActualRepayTotal, "Repay Total in the Read Me message is wrong.");


                // Loan Period Clairification (in N days)
                var sExpectedLoanPeriodClarification = "(in " + daysToNextDueDay.TotalDays.ToString("#") + " days)";
                var sActualLoanPeriodClarification = requestPage.LoanPeriodClarification;
                if ((sExpectedWantToRepay != String.Format("{0:0.00}", expectedOweToday)) && (daysToNextDueDay.Days > 0))
                {
                    Assert.AreEqual(sExpectedLoanPeriodClarification, sActualLoanPeriodClarification, "Wrong on Period Clarification, <in N days>");
                    Assert.IsTrue(requestPage.IsLoanPeriodClarificationDisplayed);
                }
                else
                {
                    Assert.IsFalse(requestPage.IsLoanPeriodClarificationDisplayed);
                }
            }
        }

        # endregion

        [Test, JIRA("UK-822"), Pending("Fails as cannot find webelement .summary-text .blue-text")]
        public void AccountOptionsCloudShown()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            //const decimal trustRating = 400.00M;

            var emailAddress1 = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(emailAddress1).Build();
            //var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(30).Build();
            ////var applicationId1 = application.Id;

            ////var accountId1 = customer.Id;

            var myAccountPage = loginPage.LoginAs(emailAddress1);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();
            mySummaryPage.CheckScenarioElementsExist();
        }
    }

}