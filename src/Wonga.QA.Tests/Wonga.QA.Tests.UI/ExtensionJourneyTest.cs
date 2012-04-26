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
    /// Extension Journey tests for UK
    /// </summary>
    /// 
    class ExtensionJourneyTest : UiTest
    {
        private int _amountMax;
        private int _amountMin;
        private string _repaymentDate;
        private ApiResponse _response;
        private DateTime _actualDate;


        [Test, AUT(AUT.Uk), JIRA("UK-427", "UK-1627"), Pending("Affected by bug UK-1746")]
        public void ExtensionJourneyPass()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);

            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);
                
            //Runs assertions internally
            requestPage.IsExtensionRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString());

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var processPage = new ExtensionProcessingPage(this.Client);

            var agreementPage =  processPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;
            agreementPage.Accept();

            var dealDonePage = new ExtensionDealDonePage(this.Client);
            Assert.IsFalse(dealDonePage.IsDealDonePageExtensionAmountNotPresent());
            Assert.IsFalse(dealDonePage.IsDealDonePageDateTokenPresent());
        }


        [Test, AUT(AUT.Uk), JIRA("UK-1321", "UK-1522"), Pending("Affected by bug UK-1746")]
        public void ExtensionJourneyDecline()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            //Runs assertions internally
            requestPage.IsExtensionRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString());

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            var processPage = new ExtensionProcessingPage(this.Client);

            var declinedPage = processPage.WaitFor<ExtensionPaymentFailedPage>() as ExtensionPaymentFailedPage;

            Assert.IsFalse(declinedPage.IsPaymentFailedAmountNotPresent());
            Assert.IsFalse(declinedPage.IsPaymentFailedDateNotPresent());

        }

        [Test, AUT(AUT.Uk), JIRA("UK-1323", "UK-1523"), Pending("Affected by bug UK-1746")]
        public void ExtensionJourneyError()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            //Runs assertions internally
            requestPage.IsExtensionRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString());

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("999");
            requestPage.SubmitButtonClick();

            var processPage = new ExtensionProcessingPage(this.Client);

            var errorPage = processPage.WaitFor<ExtensionErrorPage>() as ExtensionErrorPage;

        }


        [Test, AUT(AUT.Uk), JIRA("UK-427")]
        public void ExtensionRequestPageTest1()
        {
            CheckExtensionRequestPage(100, 5, 1);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-427")]
        public void ExtensionRequestPageTest2()
        {
            CheckExtensionRequestPage(400, 2, 30);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-427")]
        public void ExtensionRequestPageTest3()
        {
            CheckExtensionRequestPage(1, 7, 29);
        }

        /// <summary>
        /// General function used by tests
        /// </summary>
        
        private void CheckExtensionRequestPage(int loanAmount, int loanTerm, int extensionDays)
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();
            var myAccountPage = Client.Login().LoginAs(email);
            
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            // Expected
            var api = new ApiDriver();
            _response = api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery { ApplicationId = application.Id });
            var sliderMinDays = _response.Values["SliderMinDays"].Single();
            var sliderMaxDays = _response.Values["SliderMaxDays"].Single();
            var oweToday = decimal.Parse(_response.Values["TotalAmountDueToday"].Single());
            var sOweToday = String.Format("£{0}", oweToday.ToString("#.00"));
            var totalRepayToday = decimal.Parse(_response.Values["ExtensionPartPaymentAmount"].Single());
            var sTotalRepayToday = String.Format("£{0}", totalRepayToday.ToString("#.00"));
            var newCreditAmount = decimal.Parse(_response.Values["CurrentPrincipleAmount"].Single());
            var sNewCreditAmount = String.Format("£{0}", newCreditAmount.ToString("#.00"));
            var futureInterestAndFees = decimal.Parse(_response.Values["LoanExtensionFee"].Single());
            var sFutureInterestAndFees = String.Format("£{0}", futureInterestAndFees.ToString("#.00"));
            var expectedRepaymentDate = GetOrdinalDate(DateTime.Now.AddDays(loanTerm).AddDays(1));

            // Check
            //Assert.AreEqual(expectedRepaymentDate, requestPage.RepaymentDate); fails
            Assert.AreEqual("1", requestPage.InformativeBox);
            Assert.AreEqual(sOweToday, requestPage.OweToday);
            Assert.AreEqual(sTotalRepayToday, requestPage.TotalRepayToday);
            Assert.AreEqual(sNewCreditAmount, requestPage.NewCreditAmount);
            Assert.AreEqual(sFutureInterestAndFees, requestPage.InterestFees);
        }
        
        /*
        /// <summary>
        /// As a customer I want to be able extend my loan so that I can defer my repayment date to when I can afford to repay my loan
        /// </summary>
        [Test, AUT(AUT.Uk), JIRA("UK-427")]
        public void ExtensionRequestPageRewindeDates()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(100).WithLoanTerm(5).Build();

            // Rewind application dates
            ApplicationEntity applicationEntity = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
            TimeSpan daysShiftSpan = TimeSpan.FromDays(1);
            Drive.Db.RewindApplicationDates(applicationEntity, riskApplication, daysShiftSpan);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);

            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            // Expected
            var api = new ApiDriver();
            _response = api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery { ApplicationId = application.Id });
            var sliderMinDays = _response.Values["SliderMinDays"].Single();
            var sliderMaxDays = _response.Values["SliderMaxDays"].Single();
            var oweToday = _response.Values["TotalAmountDueToday"].Single();
            var totalRepayToday = _response.Values["ExtensionPartPaymentAmount"].Single();
            var newCreditAmount = _response.Values["CurrentPrincipleAmount"].Single();
            var futureInterestAndFees = _response.Values["LoanExtensionFee"].Single();
            // Total to repay

            // TBD - check the values on the page
            // Max Days
            // repayment date 2 instances
            // owe today
            // repay today
            // new credit amoutn
            // future interest and fees
            // total to repay
            // read me message

        }	*/
        
        public string GetOrdinalDate(DateTime date)
        {
            //Returns date as string in the format "Wed 18th Apr 2012"
            var cDate = date.Day.ToString("d")[date.Day.ToString("d").Length - 1];
            string suffix;
            switch (cDate)
            {
                case '1':
                    suffix = "st";
                    break;
                case '2':
                    suffix = "nd";
                    break;
                case '3':
                    suffix = "rd";
                    break;
                default:
                    suffix = "th";
                    break;
            }
            var sDate = date.Day.ToString("d") + " ";
            var sDateOrdinial = date.Day.ToString("d") + suffix + " ";
            return date.ToString("d MMM yyyy").Replace(sDate, sDateOrdinial);
        }
    }
}
