using System.Globalization;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using System.Linq;
using System;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.UiTests.Web
{
    class TopUpTests : UiTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-826", "UK-789", "UK-2016", "UKWEB-928"), MultipleAsserts, Pending("UKWEB-928: Top Up throws an exception on the Accept page")]
        [Owner(Owner.OrizuNwokeji, Owner.StanDesyatnikov)]
        //[Category("CoreTest")] // Uncommnet when the test works
        public void TopUpHappyPath()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(30).Build();

            var responseLimit = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery {AccountId = customer.Id});
            int _amountMax = (int) Decimal.Parse(responseLimit.Values["AmountMax"].Single(), CultureInfo.InvariantCulture);
            int _amountMin = 1;

            int randomAmount = _amountMin + (new Random()).Next(_amountMax - _amountMin);

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            decimal topupAmountDec = (decimal) randomAmount;
            var topupAmount = randomAmount.ToString();

            mySummaryPage.TopupSliders.HowMuch = topupAmount;

            ApiResponse _response = Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery
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
    
        [Test, AUT(AUT.Uk), JIRA("UK-789"), MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        // Check on the Top Up Request page that
        // the Available Credit is correct for various scenarios
        public void TopUpCheckAvailableCreditForScenarios()
        {
            // Create a customer and loan
            string email = Get.RandomEmail();
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).Build();
            
            // *************************************************************************
            // Rewind application dates for 1 day (scenario 1)
            // *************************************************************************
            TimeSpan daysShiftSpan = TimeSpan.FromDays(1);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            // Log in and open MySummary page
            var mySummaryPage = Client.Login().LoginAs(email);
            
            // Get expected Available Credit
            ApiResponse fixedTermLoanTopupOfferResponse = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = customer.Id });
            string expectedAvailableCredit = (int.Parse(fixedTermLoanTopupOfferResponse.Values["AmountMax"].Single().Split('.')[0])).ToString("#"); 
            
            // Compare expected Available Credit with actual values
            string actualIntroText = mySummaryPage.GetIntroText; // in Introduction Text
            string actualMaxAvailableCredit = mySummaryPage.GetMaxAvailableCredit; // near slide bars
            Assert.Contains(actualIntroText, expectedAvailableCredit, "IntroText shows wrong AvailableCredit for scenario 1");
            Assert.Contains(actualMaxAvailableCredit, expectedAvailableCredit, "Max Available Credit is wrong for scenario 1");

            // *************************************************************************
            // Rewind application dates for 3 more days (scenario 3)
            // *************************************************************************
            daysShiftSpan = TimeSpan.FromDays(3);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            // Get expected Available Credit
            fixedTermLoanTopupOfferResponse = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = customer.Id });
            expectedAvailableCredit = (int.Parse(fixedTermLoanTopupOfferResponse.Values["AmountMax"].Single().Split('.')[0])).ToString("#");

            Client.Driver.Navigate().Refresh();

            // Compare expected Available Credit with actual values
            actualIntroText = mySummaryPage.GetIntroText; // in Introduction Text
            actualMaxAvailableCredit = mySummaryPage.GetMaxAvailableCredit; // near slide bars
            Assert.Contains(actualIntroText, expectedAvailableCredit, "IntroText shows wrong AvailableCredit for scenario 3");
            Assert.Contains(actualMaxAvailableCredit, expectedAvailableCredit, "Max Available Credit is wrong for scenario 3");
        }
    }
}