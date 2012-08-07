using System.Globalization;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;
using System.Linq;
using System;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    class ExtensionTests : UiTest
    {
        private int _amountMax;
        private int _amountMin;
        //private string _repaymentDate;
        //private DateTime _actualDate;

        #region Private Methods

        private void ExtensionRequestPage(int loanAmount, int loanTerm, int extensionDays)
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();
            var mySummaryPage = Client.Login().LoginAs(email);

            mySummaryPage.ChangePromiseDateButtonClick();
            var extensionRequestPage = new ExtensionRequestPage(this.Client);


            var api = new ApiDriver();
            ApiResponse response = api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery { ApplicationId = application.Id });

            // Set Dates to extend
            //extensionRequestPage.SetInformativeBox(extensionDays);
            //Thread.Sleep(2000);

            // Set expected values
            var sliderMinDays = response.Values["SliderMinDays"].Single();
            var sliderMaxDays = response.Values["SliderMaxDays"].Single();
            var oweToday = decimal.Parse(response.Values["TotalAmountDueToday"].Single());
            var sOweToday = String.Format("£{0}", oweToday.ToString("#.00"));
            var totalRepayToday = decimal.Parse(response.Values["ExtensionPartPaymentAmount"].Single());
            var sTotalRepayToday = String.Format("£{0}", totalRepayToday.ToString("#.00"));
            var newCreditAmount = decimal.Parse(response.Values["CurrentPrincipleAmount"].Single());
            var sNewCreditAmount = String.Format("£{0}", newCreditAmount.ToString("#.00"));
            //var futureInterestAndFees = decimal.Parse(response.Values["LoanExtensionFee"].Single()); wrong
            //var sFutureInterestAndFees = String.Format("£{0}", futureInterestAndFees.ToString("#.00")); wrong

            var expectedRepaymentDate = Date.GetOrdinalDate(DateTime.Now.AddDays(loanTerm).AddDays(1), "d MMMM yyyy");

            // Main checks
            Assert.AreEqual("1", sliderMinDays);
            Assert.AreEqual("30", sliderMaxDays);
            Assert.AreEqual(expectedRepaymentDate, extensionRequestPage.RepaymentDate, "Repayment Date is incorrect."); // Repayment Date
            Assert.AreEqual(extensionDays.ToString("#"), extensionRequestPage.InformativeBox, "InformativeBox"); // Days (for extention)
            Assert.AreEqual(sOweToday, extensionRequestPage.OweToday, "OweToday"); // Owe today
            Assert.AreEqual(sTotalRepayToday, extensionRequestPage.TotalRepayToday, "TotalRepayToday"); // Total to Repay Today
            Assert.AreEqual(sNewCreditAmount, extensionRequestPage.NewCreditAmount, "NewCreditAmount"); // New Credit Amount
            Assert.AreEqual("£" + response.Values["FutureInterestAndFees"].ToArray()[extensionDays - 1], extensionRequestPage.InterestFees, "Future Interest And Fees is not correct for Extension Days={0}", extensionDays); // Future Interests and Fees
            Assert.AreEqual("£" + response.Values["TotalAmountDueOnExtensionDate"].ToArray()[extensionDays - 1], extensionRequestPage.TotalToRepay, "Total To Repay on Extension Date is not correct for Extension Days={0}", extensionDays); // Total to Repay on Extension Date
            Assert.AreEqual(Date.GetOrdinalDate(DateTime.Parse(response.Values["ExtensionDate"].ToArray()[extensionDays - 1]).Date, "d MMMM yyyy"), extensionRequestPage.RepaymentDate, "Extensions Date is not correct for Extension Days={0}", extensionDays); // Extension Date
        }

        private void ExtensionRequestPageNDaysAfterLoanTaken(int loanAmount, int loanTerm, int daysAfterLoan)
        {
            const int extensionDays = 1;
            const int extensionFee = 10;
            const Decimal transmissionFee = 5.50M;
            string email = Get.RandomEmail();
            const decimal mir = 0.3M;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();

            if (daysAfterLoan > 0)
            {
                // Rewind application dates
                ApplicationEntity applicationEntity = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
                RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
                TimeSpan daysShiftSpan = TimeSpan.FromDays(daysAfterLoan);
                Drive.Db.RewindApplicationDates(applicationEntity, riskApplication, daysShiftSpan);
            }

            // Expected
            var api = new ApiDriver();
            ApiResponse response = api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery { ApplicationId = application.Id });
            var oweToday = decimal.Parse(response.Values["TotalAmountDueToday"].Single());
            var sOweToday = String.Format("£{0}", oweToday.ToString("#.00"));
            var totalRepayToday = decimal.Parse(response.Values["ExtensionPartPaymentAmount"].Single());
            var sTotalRepayToday = String.Format("£{0}", totalRepayToday.ToString("#.00"));
            var newCreditAmount = decimal.Parse(response.Values["CurrentPrincipleAmount"].Single());
            var sNewCreditAmount = String.Format("£{0}", newCreditAmount.ToString("#.00"));

            const decimal interestPerDay = (mir * 12) / 365;
            var newLoanTerm = loanTerm - daysAfterLoan + extensionDays;
            var postedInterest = (application.LoanAmount + transmissionFee) * newLoanTerm * interestPerDay;
            var expectedTotalPayable = (application.LoanAmount + extensionFee) * (1 + Decimal.Round(newLoanTerm * interestPerDay, 2));
            var sFutureInterestAndFees = String.Format("£{0:0.00}", Decimal.Round((expectedTotalPayable - application.LoanAmount), 2));
            response = api.Queries.Post(new GetFixedTermLoanExtensionParametersQuery { AccountId = customer.Id });
            var expectedRepaymentDate = Date.GetOrdinalDate(Convert.ToDateTime(response.Values["NextDueDate"].Single()).AddDays(extensionDays), "d MMMM yyyy");

            // Log in
            var mySummaryPage = Client.Login().LoginAs(email);

            // Extend
            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            // Check
            Assert.AreEqual(expectedRepaymentDate, requestPage.RepaymentDate);
            Assert.AreEqual("1", requestPage.InformativeBox, "InformativeBox");
            Assert.AreEqual(sOweToday, requestPage.OweToday, "OweToday");
            Assert.AreEqual(sTotalRepayToday, requestPage.TotalRepayToday, "TotalRepayToday");
            Assert.AreEqual(sNewCreditAmount, requestPage.NewCreditAmount, "NewCreditAmount");
            Assert.AreEqual(sFutureInterestAndFees, requestPage.InterestFees, "InterestFees");
            Assert.AreNotEqual(requestPage.TotalToRepay, requestPage.InterestFees, "Interest Fees and Total To Repay should not be equal.");
        }

        private void ExtensionRequestPageChangeDays(int loanAmount, int loanTerm)
        {
            string email = Get.RandomEmail();
            int[] extensionDaysArray = { 2, 10, 15, 29, 30, 1 };

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();
            var mySummaryPage = Client.Login().LoginAs(email);

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            var api = new ApiDriver();
            ApiResponse response = api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery { ApplicationId = application.Id });

            // Change value in the Days field 
            //string expExtensionDate;
            //string actExtensionDate;
            foreach (int extensionDays in extensionDaysArray)
            {
                // Set Dates to extend
                requestPage.SetInformativeBox(extensionDays);
                Thread.Sleep(2000);

                // Main checks
                Assert.AreEqual("£" + response.Values["FutureInterestAndFees"].ToArray()[extensionDays - 1], requestPage.InterestFees, "Future Interest And Fees is not correct for Extension Days={0}", extensionDays); // Future Interests and Fees
                Assert.AreEqual("£" + response.Values["TotalAmountDueOnExtensionDate"].ToArray()[extensionDays - 1], requestPage.TotalToRepay, "Total To Repay on Extension Date is not correct for Extension Days={0}", extensionDays); // Total to Repay on Extension Date
                Assert.AreEqual(Date.GetOrdinalDate(DateTime.Parse(response.Values["ExtensionDate"].ToArray()[extensionDays - 1]).Date, "d MMMM yyyy"), requestPage.RepaymentDate, "Extensions Date is not correct for Extension Days={0}", extensionDays); // Extension Date
                // Extra checks    
                Assert.AreNotEqual(requestPage.TotalToRepay, requestPage.InterestFees, "Interest Fees and Total To Repay should not be equal.");
            }
        }

        private void Topup(string email, MySummaryPage mySummaryPage, Application application, Customer customer)
        {
            var responseLimit = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = customer.Id });
            _amountMin = 1;
            _amountMax = (int)Decimal.Parse(responseLimit.Values["AmountMax"].Single(), CultureInfo.InvariantCulture);
            int topupAmount = _amountMin + (new Random()).Next(_amountMax - _amountMin);

            mySummaryPage.TopupSliders.HowMuch = topupAmount.ToString("#");

            ApiResponse response = Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery { ApplicationId = application.Id, TopupAmount = topupAmount });
            var totalRepayable = response.Values["TotalRepayable"].Single();
            var interestAndFees = response.Values["InterestAndFeesAmount"].Single();
            var requestPage = mySummaryPage.TopupSliders.Apply();
            requestPage.SubmitButtonClick();
            var processPage = new TopupProcessingPage(Client);
            var agreementPage = processPage.WaitForAgreementPage(Client);
            var dealDonePage = agreementPage.Accept();
            mySummaryPage = dealDonePage.ContinueToMyAccount() as MySummaryPage;
        }

        private void Extend(string email, MySummaryPage mySummaryPage, int loanTerm, Application application, int extensionDays)
        {
            // Open Extension Request page
            mySummaryPage.ChangePromiseDateButtonClick();
            var extensionRequestPage = new ExtensionRequestPage(this.Client);

            // Get Extension application page contents
            var api = new ApiDriver();
            ApiResponse response = api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery { ApplicationId = application.Id });

            // Set Dates to extend
            extensionRequestPage.SetInformativeBox(extensionDays);
            Thread.Sleep(2000);

            // Main checks
            Assert.AreEqual("£" + response.Values["FutureInterestAndFees"].ToArray()[extensionDays - 1], extensionRequestPage.InterestFees, "Future Interest And Fees is not correct for Extension Days={0}", extensionDays); // Future Interests and Fees
            Assert.AreEqual("£" + response.Values["TotalAmountDueOnExtensionDate"].ToArray()[extensionDays - 1], extensionRequestPage.TotalToRepay, "Total To Repay on Extension Date is not correct for Extension Days={0}", extensionDays); // Total to Repay on Extension Date
            Assert.AreEqual(Date.GetOrdinalDate(DateTime.Parse(response.Values["ExtensionDate"].ToArray()[extensionDays - 1]).Date, "d MMMM yyyy"), extensionRequestPage.RepaymentDate, "Extensions Date is not correct for Extension Days={0}", extensionDays); // Extension Date

            extensionRequestPage.setSecurityCode("123"); // fill in card security code
            extensionRequestPage.SubmitButtonClick(); // click Submit & open Processing page
            var extensionProcessingPage = new ExtensionProcessingPage(this.Client);
            var agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage; // wait for Agreement page           
            agreementPage.Accept(); // accept aggreement & open Deal Done page
            var dealDonePage = new ExtensionDealDonePage(this.Client);
        }

        #endregion
        
        [Test, JIRA("UK-427", "UK-1627", "UK-1746", "UKWEB-911"), MultipleAsserts, Owner(Owner.StanDesyatnikov, Owner.OrizuNwokeji)]
        [Category(TestCategories.CoreTest)]
        public void ExtensionJourneyPass()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(2).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            //Runs assertions internally
            requestPage.IsExtensionRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString());

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

        [Test, JIRA("UK-1321", "UK-1522", "UK-1746"), MultipleAsserts, Owner(Owner.StanDesyatnikov, Owner.OrizuNwokeji)]
        public void ExtensionJourneyDecline()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            //Runs assertions internally
            requestPage.IsExtensionRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString());

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            var extensionProcessingPage = new ExtensionProcessingPage(this.Client);

            var declinedPage = extensionProcessingPage.WaitFor<ExtensionPaymentFailedPage>() as ExtensionPaymentFailedPage;

            Assert.IsFalse(declinedPage.IsPaymentFailedAmountNotPresent());
            Assert.IsFalse(declinedPage.IsPaymentFailedDateNotPresent());
        }

        [Test, JIRA("UK-1323", "UK-1523", "UK-1746"), MultipleAsserts, Owner(Owner.StanDesyatnikov, Owner.OrizuNwokeji)]
        public void ExtensionJourneyError()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            //Runs assertions internally
            requestPage.IsExtensionRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString());

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("999");
            requestPage.SubmitButtonClick();

            var extensionProcessingPage = new ExtensionProcessingPage(Client);

            var errorPage = extensionProcessingPage.WaitFor<ExtensionErrorPage>() as ExtensionErrorPage;
        }

        [Test, JIRA("UK-427", "UK-1739", "UK-2121"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        [Row(100, 5, 1)]
        [Row(400, 2, 1)]
        [Row(400, 7, 1)]
        [Row(1, 7, 1)]
        [Row(1, 2, 1)]
        public void ExtensionRequestPageInitialValuesTest(int loanAmount, int loanTerm, int extensionDays)
        {
            ExtensionRequestPage(loanAmount, loanTerm, extensionDays);
        }

        [Test, JIRA("UK-427"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void ExtensionRequestPageChangeExtensionDaysFieldTest()
        {
            int loanAmount = 1;
            int loanTerm = 7;
            ExtensionRequestPageChangeDays(loanAmount, loanTerm);
        }

        [Test, JIRA("UK-427", "Uk-1862", "UK-2121", "UKWEB-304"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        [Pending("UKWEB-304: Incorrect Future Interest and Fees in Extension afer N days after application")]
        [Row(1, 2, 1)]
        [Row(1, 7, 1)] // UKWEB-304
        [Row(10, 7, 6)] // UKWEB-304
        public void ExtensionRequestPageNDaysAfterLoanTakenTest(int loanAmount, int loanTerm, int daysAfterLoan)
        {
            ExtensionRequestPageNDaysAfterLoanTaken(loanAmount, loanTerm, daysAfterLoan);
        }

        [Test, JIRA("UK-427"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        [Pending("Test in development")]
        public void ExtensionRequestPage1TopupAnd1ExtendTest()
        {
            string email = Get.RandomEmail();
            int loanTerm = 7;
            int extensionDays = 10;
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(100).WithLoanTerm(loanTerm).Build();
            var mySummaryPage = Client.Login().LoginAs(email);

            Topup(email, mySummaryPage, application, customer);
            Extend(email, mySummaryPage, loanTerm, application, extensionDays);
        }

        [Test, JIRA("UK-427", "UK-1862", "UK-1859"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void TotalPayableFutureInterestAndFeesTest()
        {
            const int extensionDays = 1;
            const int extensionFee = 10;
            const int loanAmount = 100;
            const int loanTerm = 5;
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();
            var mySummaryPage = Client.Login().LoginAs(email);

            // Open Extension Request page
            mySummaryPage.ChangePromiseDateButtonClick();
            var extensionRequestPage = new ExtensionRequestPage(this.Client);

            // Get contents of Extension Request page
            var api = new ApiDriver();
            ApiResponse response = api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery { ApplicationId = application.Id });

            // Set Dates to extend
            extensionRequestPage.SetInformativeBox(extensionDays);
            Thread.Sleep(2000);

            // Expected
            var newloanTerm = loanTerm + extensionDays;
            var expectedTotalPayable = (application.LoanAmount + extensionFee) * (1 + (newloanTerm * 0.00986301369863m));
            var expectedFutureInterestAndFees = Decimal.Round(expectedTotalPayable - application.LoanAmount, 2);
            // Actuals
            var actualTotalPayable = response.Values["TotalAmountDueOnExtensionDate"].ToArray()[extensionDays - 1];
            var actualFutureInterestAndFees = response.Values["FutureInterestAndFees"].ToArray()[extensionDays - 1];

            Assert.IsTrue(
                (expectedTotalPayable.ToString("#.##") == actualTotalPayable) &&
                (expectedFutureInterestAndFees.ToString("#.##") == actualFutureInterestAndFees),
                "expectedTotalPayable={0} vs. " +
                "actualTotalPayable={1}\n" +
                "expectedFutureInterestAndFees={2} vs." +
                "actualFutureInterestAndFees={3}",
                expectedTotalPayable.ToString("#.##"), actualTotalPayable, expectedFutureInterestAndFees, actualFutureInterestAndFees);
        }
    }
}
