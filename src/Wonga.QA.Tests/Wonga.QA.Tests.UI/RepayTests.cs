using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Elements;
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
    class RepayTests : UiTest
    {
        //private int _amountMax;
        //private int _amountMin;
        //private string _repaymentDate;
        //private ApiResponse _response;
        //private DateTime _actualDate;
       
        
        [Test, AUT(AUT.Uk), JIRA("UK-1827"), Pending ("In development")]
        public void MovingRepaySliderRemainingAmountShouldBeCorrect()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Runs assertions internally
            requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString(), "50");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1827")]
        public void DefaultRepayPageValuesAreCorrect()
        {
            // Build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();
            
            // Open Repay Request page
            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);
            
            // TBD - Check default values
            
            // You currently owe
            //var expectedOweToday = application.GetBalance();
            var expectedOweToday = application.GetBalanceToday();
            string sExpectedOweToday = String.Format("{0:0.00}", expectedOweToday);
            string sActualOweToday = requestPage.OweToday.TrimStart('£');
            Assert.AreEqual(sExpectedOweToday, sActualOweToday, "Currently Owe is wrong.");
            
            // How much do you want to repay? slider's current status, value in the box
            var expectedWantToRepay = expectedOweToday;
            string sExpectedWantToRepay = sExpectedOweToday;
            string sActualWantToRepay = requestPage.WantToRepayBox;
            Assert.AreEqual(sExpectedWantToRepay, sActualWantToRepay, "Want to Repay is wrong.");

            // Remainder to repay = Amount Owed - Repay Amout
            var expectedReminderToRepay = expectedOweToday - expectedWantToRepay;
            string sExpectedReminderToRepay = String.Format("{0:0.00}", expectedReminderToRepay);
            string sActualReminderToRepay = requestPage.RemainderAmount.TrimStart('£');
            Assert.AreEqual(sExpectedReminderToRepay, sActualReminderToRepay, "Reminder Amount is wrong.");

            // Repay Total in the Read Me message
            string sActualRepayTotal = requestPage.RepayTotal.TrimStart('£');
            Assert.AreEqual(sExpectedWantToRepay, sActualRepayTotal, "Repay Total in the Read Me message is wrong.");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1827")]
        public void ChangeWantToRepayBox()
        {

            var AmountToRepayMinimum = 5;
            // Build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            
            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            // Open Repay Request page
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
            var sliders = new SmallRepaySlidersElement(requestPage) {HowMuch = "1"};
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

        [Test, AUT(AUT.Uk), JIRA("UK-1827")]
        public void ClickingCancelOnRepayPageOpensMySummaryPage()
        {
            // Build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            // Open Repay Request page
            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            // Click Cancel button
            requestPage.CancelButtonClick();
        }

        // TBD
        // Check for scenarion 3, 4, 6, 7
        [Test, AUT(AUT.Uk), JIRA("UK-1827"), Pending("In development")]
        [Row(3)]
        [Row(4)]
        [Row(6)]
        [Row(7)]
        public void CheckRepaymentVsScenarios(int scenarioId)
        {
            //RepaymentVsScenarios(scenarioId);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1827"), Pending("In development")]
        public void RepayEarlyFull()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayEarlyFullpaySuccessPage>() as RepayEarlyFullpaySuccessPage;

            Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageAmountTokenNotPresent());
            Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageDateTokenNotPresent());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1827"), Pending("In development")]
        public void RepayDueFull()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 7;

            //time-shift loan so it's due today
            var applicationsTab = Drive.Data.Payments.Db.Applications;
            dynamic applicationEntity = applicationsTab.FindAll(applicationsTab.ExternalId == application.Id).Single();
            var riskApplicationsTab = Drive.Data.Risk.Db.RiskApplications;
            dynamic riskApplicationEntity = riskApplicationsTab.FindAll(riskApplicationsTab.ApplicationId == application.Id).Single(); 
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(applicationEntity, riskApplicationEntity, daysShiftSpan);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayDueFullpaySuccessPage>() as RepayDueFullpaySuccessPage;

        }

        [Test, AUT(AUT.Uk), JIRA("UK-1827"), Pending("In development")]
        public void RepayOverdueFull()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 9;

            //time-shift loan so it's due today
            var applicationsTab = Drive.Data.Payments.Db.Applications;
            dynamic applicationEntity = applicationsTab.FindAll(applicationsTab.ExternalId == application.Id).Single();
            var riskApplicationsTab = Drive.Data.Risk.Db.RiskApplications;
            dynamic riskApplicationEntity = riskApplicationsTab.FindAll(riskApplicationsTab.ApplicationId == application.Id).Single(); 
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(applicationEntity, riskApplicationEntity, daysShiftSpan);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            //requestPage.SubmitButtonClick();

            //var repayProcessingPage = new RepayProcessingPage(this.Client);

            //var paymentTakenPage = repayProcessingPage.WaitFor<RepayOverdueFullpaySuccessPage>() as RepayOverdueFullpaySuccessPage;

        }
        [Test, AUT(AUT.Uk), JIRA("UK-1827"), Pending("In development")]
        public void RepayEarlyPart()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Set partial payment amount, test for correct values at same time
            requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString(), "100");

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayEarlyPartpaySuccessPage>() as RepayEarlyPartpaySuccessPage;

            Assert.IsFalse(paymentTakenPage.IsRepayEarlyPartpaySuccessPageAmountTokenNotPresent());
            Assert.IsFalse(paymentTakenPage.IsRepayEarlyPartpaySuccessPageDateTokenNotPresent());
        }

    }
}