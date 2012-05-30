using System.Globalization;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.UI;
using Wonga.QA.Tests.Core;
using System.Linq;
using System;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Tests.Ui
{
    /// <summary>
    /// TopupSlider tests for UK
    /// </summary>
    /// 
    [Parallelizable(TestScope.All)]
    class TopupSliderTests : UiTest
    {
        private int _amountMax;
        private int _amountMin;
        //private string _repaymentDate;
        private ApiResponse _response;
        //private DateTime _actualDate;


        [Test, AUT(AUT.Uk), JIRA("UK-826", "UK-789", "UK-2016"), Pending("Fails due to bug UK-2016")]
        public void MovingTopupSlidersLoanSummaryShouldBeCorrect()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(30).Build();

            var responseLimit = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery {AccountId = customer.Id});
            _amountMax = (int) Decimal.Parse(responseLimit.Values["AmountMax"].Single(), CultureInfo.InvariantCulture);
            _amountMin = 1;

            int randomAmount = _amountMin + (new Random()).Next(_amountMax - _amountMin);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            decimal topupAmountDec = (decimal) randomAmount;
            var topupAmount = randomAmount.ToString();

            mySummaryPage.TopupSliders.HowMuch = topupAmount;

            _response =
                Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery
                                           {ApplicationId = application.Id, TopupAmount = topupAmountDec});
            var totalRepayable = _response.Values["TotalRepayable"].Single();
            var interestAndFees = _response.Values["InterestAndFeesAmount"].Single();

            Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalToRepay.Remove(0, 1), totalRepayable);
            Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalAmount.Remove(0, 1), topupAmount);
            Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalFees.Remove(0, 1), interestAndFees);

            var requestPage =
                mySummaryPage.TopupSliders.Apply();

            //Runs assertions internally
            requestPage.IsTopupRequestPageSliderReturningCorrrectValuesOnChange(application.Id.ToString());

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

        [Test, AUT(AUT.Uk), JIRA("UK-789")]
        public void CheckAvailableCreditScenario02() { CheckAvailableCredit(1); }

        [Test, AUT(AUT.Uk), JIRA("UK-789")]
        public void CheckAvailableCreditScenario03() { CheckAvailableCredit(4); }

        [Test, AUT(AUT.Uk), JIRA("UK-789"), Pending("Disabled until scenario 4 conditions are clarified")]
        public void CheckAvailableCreditScenario04() { CheckAvailableCredit(10); } 

        private void CheckAvailableCredit(int daysShift)
        {
            // Create a customer and loan
            string email = Get.RandomEmail();
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).Build();

            // Rewind application dates
            // Rewind application dates
            ApplicationEntity applicationEntity = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            Drive.Db.RewindApplicationDates(applicationEntity, riskApplication, daysShiftSpan);

            // Log in and open MySummary page
            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();
            
            // Get expected Available Credit
            ApiResponse FixedTermLoanTopupOfferResponse = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = customer.Id });
            string expectedAvailableCredit = (int.Parse(FixedTermLoanTopupOfferResponse.Values["AmountMax"].Single().Split('.')[0])).ToString("#"); 
            
            // Compare expected Available Credit with actual values
            string actualIntroText = mySummaryPage.GetIntroText; // in Introduction Text
            string actualMaxAvailableCredit = mySummaryPage.GetMaxAvailableCredit; // near slide bars
            Assert.Contains(actualIntroText, expectedAvailableCredit);
            Assert.Contains(actualMaxAvailableCredit, expectedAvailableCredit);
        }
    }
}