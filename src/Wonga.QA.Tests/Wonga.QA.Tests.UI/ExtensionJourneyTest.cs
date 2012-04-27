using System.Collections.Generic;
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
            CheckExtensionRequestPage(100, 5);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-427")]
        public void ExtensionRequestPageTest2()
        {
            CheckExtensionRequestPage(400, 2);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-427")]
        public void ExtensionRequestPageTest3()
        {
            CheckExtensionRequestPage(1, 7);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-427")]
        public void ExtensionRequestPageTest4()
        {
            CheckExtensionRequestPageNextDay(1, 7);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-427")]
        public void ExtensionRequestPageChangeDaysTest5()
        {
            CheckExtensionRequestPageChangeDays(1, 7);
        }


        [Test, AUT(AUT.Uk), JIRA("UK-427", "UK-1746"), Pending("Uncomment Assert for RepaymentDate when UK-1746 is fixed.")]
        public void ExtensionRequestPageRepaymentDateTest() {}

        [Test, AUT(AUT.Uk), JIRA("UK-427", "UK-1739"), Pending("Uncomment Assert for OweToday when UK-1739 is fixed.")]
        public void ExtensionRequestPageOweTodayTest() { }

        /// <summary>
        /// General function used by tests
        /// </summary>
        
        private void CheckExtensionRequestPage(int loanAmount, int loanTerm)
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
            var expectedRepaymentDate = GetOrdinalDate(DateTime.Now.AddDays(loanTerm).AddDays(1), "d MMM yyyy");

            // Check
            //Assert.AreEqual(expectedRepaymentDate, requestPage.RepaymentDate); fails UK-1746
            Assert.AreEqual("1", requestPage.InformativeBox, "InformativeBox");
            //Assert.AreEqual(sOweToday, requestPage.OweToday, "OweToday"); fails UK-1739
            Assert.AreEqual(sTotalRepayToday, requestPage.TotalRepayToday, "TotalRepayToday");
            Assert.AreEqual(sNewCreditAmount, requestPage.NewCreditAmount, "NewCreditAmount");
            Assert.AreEqual(sFutureInterestAndFees, requestPage.InterestFees, "InterestFees");
        }


        private void CheckExtensionRequestPageNextDay(int loanAmount, int loanTerm)
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();

            // Rewind application dates
            ApplicationEntity applicationEntity = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
            TimeSpan daysShiftSpan = TimeSpan.FromDays(2);
            Drive.Db.RewindApplicationDates(applicationEntity, riskApplication, daysShiftSpan);

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
            var expectedRepaymentDate = GetOrdinalDate(DateTime.Now.AddDays(loanTerm).AddDays(1), "d MMM yyyy");

            // Check
            //Assert.AreEqual(expectedRepaymentDate, requestPage.RepaymentDate); fails UK-1746
            Assert.AreEqual("1", requestPage.InformativeBox, "InformativeBox");
            //Assert.AreEqual(sOweToday, requestPage.OweToday, "OweToday"); fails UK-1739
            Assert.AreEqual(sTotalRepayToday, requestPage.TotalRepayToday, "TotalRepayToday");
            Assert.AreEqual(sNewCreditAmount, requestPage.NewCreditAmount, "NewCreditAmount");
            Assert.AreEqual(sFutureInterestAndFees, requestPage.InterestFees, "InterestFees");
        }


        private void CheckExtensionRequestPageChangeDays(int loanAmount, int loanTerm)
        {
            string email = Get.RandomEmail();
            int[] extensionDaysArray = { 2, 10, 15, 29, 30, 1 };

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();
            var myAccountPage = Client.Login().LoginAs(email);

            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);
            
            var api = new ApiDriver();
            // Expected
            _response = api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery { ApplicationId = application.Id });

            // Scroll sliders - TBD
            // Change value in the Days field 
            string expExtensionDate;
            string actExtensionDate;
            foreach (int extensionDays in extensionDaysArray)
            {
                requestPage.SetInformativeBox(extensionDays);
                Thread.Sleep(2000);
                expExtensionDate = GetOrdinalDate(DateTime.Parse(_response.Values["ExtensionDate"].ToArray()[extensionDays - 1]).Date, "d MMMM yyyy");
                actExtensionDate = requestPage.RepaymentDate;
                Assert.AreEqual(expExtensionDate, actExtensionDate, "Extend Date Incorrect {0}", extensionDays);
                //Assert.AreEqual(_response.Values["TotalAmountDueOnExtensionDate"].ToArray()[0], actTotalAmountAndFees, "First Day Extend Date Incorrect");
            }               
        }

        public string GetOrdinalDate(DateTime date, string format)
        {
            var cDate = date.Day.ToString("d")[date.Day.ToString("d").Length - 1];
            string suffix;
            if ((date.Day.ToString("d").Length == 2) && (date.Day.ToString("d")[0] == '1'))
                suffix = "th";
            else
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
            return date.ToString(format).Replace(sDate, sDateOrdinial);
        }
    }
}
