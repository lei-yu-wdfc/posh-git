using System;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;

namespace Wonga.QA.Tests.Migration
{
    class MigrationUiTests: UiTest
    {
        private int _amountMax;
        private int _amountMin;
        //private string _repaymentDate;
        private ApiResponse _response;

        public MigrationUiTests(ApiResponse response)
        {
            _response = response;
        }

        //private DateTime _actualDate;
        
        [Test, Owner(Owner.StanDesyatnikov)]
        public void MigratedCustomerTakesLoanTest()
        {
            string email = GetMigratedEmail();
            
            // Log in asn an existing migrated V2 customer
            Client.Login().LoginAs(email);

            // Take a loan 
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var mySummary = journey.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, AUT(AUT.Za), Pending("in development, test environments still not ready")]
        public void MigExtensionJourneyPass()
        {
            string email = GetMigratedEmail();

            //var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            //var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(2).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);

            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            //Runs assertions internally
            //requestPage.IsExtensionRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString());

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var extensionProcessingPage = new ExtensionProcessingPage(this.Client);

            var agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;
            agreementPage.Accept();

            var dealDonePage = new ExtensionDealDonePage(this.Client);
            Assert.IsFalse(dealDonePage.IsDealDonePageExtensionAmountNotPresent());
            Assert.IsFalse(dealDonePage.IsDealDonePageDateTokenPresent());
        }

        private string GetMigratedEmail()
        {
            // var acctTab = Drive.Data.Ops.Db.Accounts;
            // string email = acctTab.FindbyInitialDataMigratedOn("not null").Single();
            // return email;

            return "qa.wonga.com+TEAMCITY8-fc928b6e-6c4f-4998-b949-0375a469aa8c@gmail.com";
        }

        private string GetMigratedAcct()
        {
            var accountTab = Drive.Data.Ops.Db.Accounts;

            return "";
        }

        [Test, AUT(AUT.Uk), Pending("in development, test environments still not ready"), Owner(Owner.MuhammadQureshi)]
        public void MigMovingTopupSlidersLoanSummaryShouldBeCorrect()
        {
            string email = GetMigratedEmail();

            //var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            //var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(30).Build();

            var responseLimit = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = GetMigratedAcct() });
            _amountMax = (int)Decimal.Parse(responseLimit.Values["AmountMax"].Single(), CultureInfo.InvariantCulture);
            _amountMin = 1;

            int randomAmount = _amountMin + (new Random()).Next(_amountMax - _amountMin);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            decimal topupAmountDec = (decimal)randomAmount;
            var topupAmount = randomAmount.ToString();

            mySummaryPage.TopupSliders.HowMuch = topupAmount;

            //_response =
              //  Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery { ApplicationId = application.Id, TopupAmount = topupAmountDec });
            var totalRepayable = _response.Values["TotalRepayable"].Single();
            var interestAndFees = _response.Values["InterestAndFeesAmount"].Single();

            Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalToRepay.Remove(0, 1), totalRepayable);
            Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalAmount.Remove(0, 1), topupAmount);
            Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalFees.Remove(0, 1), interestAndFees);

            var requestPage =
                mySummaryPage.TopupSliders.Apply();

            //Runs assertions internally
            //requestPage.IsTopupRequestPageSliderReturningCorrrectValuesOnChange(application.Id.ToString());

            requestPage.SubmitButtonClick();

            var processPage = new TopupProcessingPage(this.Client);
            var agreementPage = processPage.WaitForAgreementPage(Client);

            Assert.IsFalse(agreementPage.IsTopupAgreementPageDateNotPresent());
            Assert.IsTrue(agreementPage.IsTopupAgreementPageLegalInfoDisplayed());
            Assert.IsFalse(agreementPage.IsTopupAgreementPageTopupAmountNotPresent());
            Assert.IsFalse(agreementPage.IsTopupTotalAmountTokenBeingReplaced());

            var dealDonePage = agreementPage.Accept();
            Assert.IsFalse(dealDonePage.IsDealDonePageDateNotPresent());
            Assert.IsFalse(dealDonePage.IsDealDonePageJiffyNotPresent());
            Assert.IsFalse(dealDonePage.IsDealDonePageTopupAmountNotPresent());
            Assert.Contains(dealDonePage.SucessMessage, totalRepayable);
            Assert.Contains(dealDonePage.SucessMessage, topupAmount);

            dealDonePage.ContinueToMyAccount();

            //Test my account summary page
            Assert.IsTrue(this.Client.Driver.Url.Contains("my-account"));
        }

        [Test, AUT(AUT.Uk), Pending("in development, test environments still not ready")]
        public void RepayDueFull()
        {
            //build L0 loan
            //string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            //var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            //var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 7;

            //time-shift loan so it's due today
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            //ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var loginPage = Client.Login();
            //var myAccountPage = loginPage.LoginAs(email);
            //var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            //mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayDueFullpaySuccessPage>() as RepayDueFullpaySuccessPage;

        }

        [Test, AUT(AUT.Uk), Pending("in development, test environments still not ready")]
        public void RepayDuePart()
        {
            //build L0 loan
            string email = GetMigratedEmail();
            DateTime todayDate = DateTime.Now;

            //var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            //var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 7;

            //time-shift loan so it's due today
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            //ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Set partial payment amount, test for correct values at same time
            //requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString(), "100");

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayDuePartpaySuccessPage>() as RepayDuePartpaySuccessPage;
            Assert.IsFalse(paymentTakenPage.IsRepayDuePartpaySuccessPageAmountTokenNotPresent());


        }

        [Test, AUT(AUT.Uk), Pending("in development, test environments still not ready")]
        public void RepayEarlyFull()
        {
            //build L0 loan
            string email = GetMigratedEmail();
            DateTime todayDate = DateTime.Now;

            //var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            //var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();
            //var originalRepay = mySummaryPage.GetTotalToRepay();
            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayEarlyFullpaySuccessPage>() as RepayEarlyFullpaySuccessPage;

            Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageAmountTokenNotPresent());
            Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageDateTokenNotPresent());
            //TODO: Check that Interest Saved calculation is correct
        }

        [Test, AUT(AUT.Uk), Pending("in development, test environments still not ready")]
        public void RepayEarlyPart()
        {
            //build L0 loan
            string email = GetMigratedEmail();
            DateTime todayDate = DateTime.Now;

            //var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            //var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Set partial payment amount, test for correct values at same time
            //requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString(), "100");

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayEarlyPartpaySuccessPage>() as RepayEarlyPartpaySuccessPage;

            Assert.IsFalse(paymentTakenPage.IsRepayEarlyPartpaySuccessPageAmountTokenNotPresent());
            Assert.IsFalse(paymentTakenPage.IsRepayEarlyPartpaySuccessPageDateTokenNotPresent());
        }

        [Test, AUT(AUT.Uk), Pending("in development, test environments still not ready")]
        public void RepayOverdueFull()
        {
            //build L0 loan
            string email = GetMigratedEmail();
            DateTime todayDate = DateTime.Now;

            //var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            //var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 15;

            //time-shift loan so it's in arrears
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            //ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayOverduePartpaySuccessPage>() as RepayOverduePartpaySuccessPage;
            Assert.IsFalse(paymentTakenPage.IsRepayOverduePartpaySuccessPageAmountTokenNotPresent());
        }

        [Test, AUT(AUT.Uk), Pending("in development, test environments still not ready")]
        public void RepayOverduePart()
        {
            //build L0 loan
            string email = GetMigratedEmail();
            DateTime todayDate = DateTime.Now;

            //var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            //var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 15;

            //time-shift loan so it's in arrears
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            //ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Set partial payment amount, test for correct values at same time
            //requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString(), "100");

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayOverduePartpaySuccessPage>() as RepayOverduePartpaySuccessPage;
            Assert.IsFalse(paymentTakenPage.IsRepayOverduePartpaySuccessPageAmountTokenNotPresent());
        }      

    }
}
