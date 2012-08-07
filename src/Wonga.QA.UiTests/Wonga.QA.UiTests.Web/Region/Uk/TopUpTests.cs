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

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    class TopUpTests : UiTest
    {

        [Test, JIRA("QA-341"), Owner(Owner.MihailPodobivsky)]
        public void CustomerOnTheDayBeforeDueDateShouldntBeAbleToTakeExtraCash()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .WithLoanTerm(30)
                .Build();
            application.RewindApplicationDatesForDays(29);
            var mySummaryPage = loginPage.LoginAs(email);
            Assert.IsFalse(mySummaryPage.LookForTopupSliders());
        }

        [Test, JIRA("QA-342"), Owner(Owner.MihailPodobivsky)]
        public void CustomerInArrearsShoudntBeAbleToTakeExtraCredit()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.PutIntoArrears(10);
            var mySummaryPage = loginPage.LoginAs(email);
            Assert.IsFalse(mySummaryPage.LookForTopupSliders());
        }

        [Test, JIRA("UK-826", "UK-789", "UK-2016", "UKWEB-928"), MultipleAsserts]
        [Owner(Owner.OrizuNwokeji, Owner.StanDesyatnikov)]
        [Category(TestCategories.CoreTest)]
        [Pending("UKWEB-928: Top Up throws an exception on the Accept page")]
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

            Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalToRepay.Remove(0, 1), totalRepayable, "Total to Repay on Sliders on My Summary page is wrong");
            Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalAmount.Remove(0, 1), topupAmount, "Total Amount on Sliders on My Summary page is wrong");
            Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalFees.Remove(0, 1), interestAndFees, "Interest Fees on Sliders on My Summary page is wrong");

            var requestPage =
                mySummaryPage.TopupSliders.Apply();

            //Runs assertions internally
            requestPage.IsTopupRequestPageSliderReturningCorrrectValuesOnChange(application.Id.ToString());

            requestPage.SubmitButtonClick();
            
            var processPage = new TopupProcessingPage(this.Client);
            var agreementPage = processPage.WaitForAgreementPage(Client);

            Assert.IsFalse(agreementPage.IsTopupAgreementPageDateNotPresent(), "Date on Agreement page is not displayed");
            Assert.IsTrue(agreementPage.IsTopupAgreementPageLegalInfoDisplayed(), "Legal Info on Agreement page is not displayed");
            Assert.IsFalse(agreementPage.IsTopupAgreementPageTopupAmountNotPresent(), "Topup Amount on Agreement page is not displayed");
            Assert.IsFalse(agreementPage.IsTopupTotalAmountTokenBeingReplaced(), "Amount Token Agreement page is not replaced with value");

            var dealDonePage = agreementPage.Accept();
            Assert.IsFalse(dealDonePage.IsDealDonePageDateNotPresent(), "Date on Deal Done page is not displayed");
            Assert.IsFalse(dealDonePage.IsDealDonePageJiffyNotPresent(), "Jiffy on Deal Done page is not displayed");
            Assert.IsFalse(dealDonePage.IsDealDonePageTopupAmountNotPresent(), "Topup Amount on Deal Done page is not displayed");
            Assert.Contains(dealDonePage.SucessMessage, totalRepayable, "Success Message on Deal Done page does not contain Total Repayable");
            Assert.Contains(dealDonePage.SucessMessage, topupAmount, "Success Message on Deal Done page does not contain Total Amount");

            dealDonePage.ContinueToMyAccount();
            
            //Test my account summary page
            Assert.IsTrue(this.Client.Driver.Url.Contains("my-account"), "My Account page was not open");
        }
    
        [Test, JIRA("UK-789"), MultipleAsserts]
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

        [Test, JIRA("QA-340")]
        [Owner(Owner.PetrTarasenko)]
        public  void TopUpExtraCashequalToChoosen()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(30).Build();
            application.RewindApplicationDatesForDays(20);
            
            var responseLimit = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = customer.Id });
            int _amountMax = (int)Decimal.Parse(responseLimit.Values["AmountMax"].Single(), CultureInfo.InvariantCulture);
            int _amountMin = 1;
            
            int randomAmount = _amountMin + (new Random()).Next(_amountMax - _amountMin);
            
            var topupAmount = randomAmount.ToString();
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
            mySummaryPage.TopupSliders.HowMuch = topupAmount;
            var requestPage =mySummaryPage.TopupSliders.Apply();
            String[] s = requestPage.Sliders.GetTopUpAmount.Remove(0, 1).Split('.');
            Assert.AreEqual(topupAmount, requestPage.Sliders.HowMuch);
            Assert.AreEqual(topupAmount, s[0]);
          }
    }
}