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
using CreateScheduledPaymentRequestCommand = Wonga.QA.Framework.Msmq.CreateScheduledPaymentRequestCommand;


namespace Wonga.QA.Tests.Ui
{
    /// <summary>
    /// TopupSlider tests for UK
    /// </summary>
    /// 
    [Parallelizable(TestScope.All)]
    class RepayTests : UiTest
    {
        //private int _amountMax;
        //private int _amountMin;
        //private string _repaymentDate;
        //private ApiResponse _response;
        //private DateTime _actualDate;
       
        [Test, AUT(AUT.Uk), JIRA("UKWEB-247")]
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

        [Test, AUT(AUT.Uk), JIRA("UKWEB-247")]
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

        [Test, AUT(AUT.Uk), JIRA("UKWEB-247"), Pending("Fails. To be investigated.")]
        public void ClickingCancelOnRepayPageOpensMySummaryPage()
        {
            var loginPage = Client.Login();

            // Build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

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
        [Test, AUT(AUT.Uk), JIRA("UKWEB-247"), Pending("In development")]
        [Row(3)]
        [Row(4)]
        [Row(6)]
        [Row(7)]
        public void CheckRepaymentVsScenarios(int scenarioId)
        {
            //RepaymentVsScenarios(scenarioId);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1833", "UKWEB-244")]
        public void RepayEarlyDecline()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
                      
            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var declinedPage = repayProcessingPage.WaitFor<RepayEarlyPaymentFailedPage>() as RepayEarlyPaymentFailedPage;

            Assert.IsFalse(declinedPage.IsPaymentFailedAmountNotPresent());
            Assert.IsFalse(declinedPage.IsPaymentFailedDateNotPresent());
        }


        [Test, AUT(AUT.Uk), JIRA("UK-1833", "UKWEB-244")]
        public void RepayDueDecline()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            var daysShift = 9;
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(daysShift).Build();
            
            TimeSpan daysShiftSpan = TimeSpan.FromDays((Double)daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var declinedPage = repayProcessingPage.WaitFor<RepayDuePaymentFailedPage>() as RepayDuePaymentFailedPage;

            Assert.IsFalse(declinedPage.IsPaymentFailedAmountNotPresent());
            Assert.IsFalse(declinedPage.IsPaymentFailedDateNotPresent());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1833", "UKWEB-244")]
        public void RepayOverdueDecline()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            //DateTime todayDate = DateTime.Now;
            var loanTerm = 7;
            var daysShift = 15;
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(loanTerm).Build();

            TimeSpan daysShiftSpan = TimeSpan.FromDays((Double)daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var declinedPage = repayProcessingPage.WaitFor<RepayOverduePaymentFailedPage>() as RepayOverduePaymentFailedPage;

            Assert.IsFalse(declinedPage.IsPaymentFailedAmountNotPresent());
        }


        [Test, AUT(AUT.Uk), JIRA("UKWEB-247")]
        public void RepayError()
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
            requestPage.setSecurityCode("999");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentErrorPage = repayProcessingPage.WaitFor<RepayErrorPage>() as RepayErrorPage;
        }
        
          
        [Test, AUT(AUT.Uk), JIRA("UKWEB-247")]
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
        
        [Test, AUT(AUT.Uk), JIRA("UKWEB-247")]
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
            //var originalRepay = mySummaryPage.GetTotalToRepay();
            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            Assert.IsNotEmpty(requestPage.RepayCard);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayEarlyFullpaySuccessPage>() as RepayEarlyFullpaySuccessPage;

            Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageAmountTokenNotPresent());
            Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageDateTokenNotPresent());
            //TODO: Check that Interest Saved calculation is correct
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-247")]
        public void RepayEarlyPart()
        {
            // Build L0 loan
            string email = Get.RandomEmail();
            const decimal repayAmount = 100.00m;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            ApiResponse response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = application.Id });

            var dueDate = Convert.ToDateTime(response.Values["NextDueDate"].Single());
            var sDueDate = Date.GetOrdinalDate(dueDate, "d MMM yyyy");
            var oldDueDateBalance = Convert.ToDecimal(response.Values["BalanceNextDueDate"].Single());

            Client.Login().LoginAs(email).RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            // Set partial payment amount, test for correct values at same time
            requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString(), repayAmount.ToString("#"));

            // Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayEarlyPartpaySuccessPage>() as RepayEarlyPartpaySuccessPage;

            Assert.IsFalse(paymentTakenPage.IsRepayEarlyPartpaySuccessPageAmountTokenNotPresent());
            Assert.IsFalse(paymentTakenPage.IsRepayEarlyPartpaySuccessPageDateTokenNotPresent());

            // Post payment values
            ApiResponse summaryResponse = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });
            var newDueDateAmount = summaryResponse.Values["CurrentLoanRepaymentAmountOnDueDate"].Single();
            var newDueDateAmountDecimal = decimal.Parse(newDueDateAmount);
            var interestSaved = oldDueDateBalance - newDueDateAmountDecimal - repayAmount;
            string sInterestSaved = String.Format("{0:0.00}", interestSaved);
            string sNewDueDateAmount = String.Format("{0:0.00}", newDueDateAmount);

            // Get the content from the Payment Taken Page
            string paymentTakenText = paymentTakenPage.ContentArea();

            Assert.IsTrue(paymentTakenText.Contains(sNewDueDateAmount), "New Due Date Amount is wrong.");
            Assert.IsTrue(paymentTakenText.Contains(sInterestSaved), "Interest Saved is wrong.");
            Assert.IsTrue(paymentTakenText.Contains(sDueDate), "Due Date is wrong.");
            Assert.IsTrue(paymentTakenText.Contains(String.Format("{0:0.00}", repayAmount)), "Repay Amount is wrong.");
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-247")]
        public void RepayDueFull()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 7;

            ApiResponse response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = application.Id });

            var dueDate = Convert.ToDateTime(response.Values["NextDueDate"].Single());
            var sDueDate = Date.GetOrdinalDate(dueDate, "d MMM yyyy");
            var oldDueDateBalance = Convert.ToDecimal(response.Values["BalanceNextDueDate"].Single());

            //time-shift loan so it's due today
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            Client.Login().LoginAs(email).RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayDueFullpaySuccessPage>() as RepayDueFullpaySuccessPage;

            // Post payment values
            ApiResponse summaryResponse = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });
            var newDueDateAmount = summaryResponse.Values["CurrentLoanRepaymentAmountOnDueDate"].Single();
            var newDueDateAmountDecimal = decimal.Parse(newDueDateAmount);
            var interestSaved = oldDueDateBalance - newDueDateAmountDecimal - repayAmount;
            string sInterestSaved = String.Format("{0:0.00}", interestSaved);
            string sNewDueDateAmount = String.Format("{0:0.00}", newDueDateAmount);

            // Get the content from the Payment Taken Page
            string paymentTakenText = paymentTakenPage.ContentArea();

            Assert.IsTrue(paymentTakenText.Contains(sNewDueDateAmount), "New Due Date Amount is wrong.");
            Assert.IsTrue(paymentTakenText.Contains(sInterestSaved), "Interest Saved is wrong.");
            Assert.IsTrue(paymentTakenText.Contains(sDueDate), "Due Date is wrong.");
            Assert.IsTrue(paymentTakenText.Contains(String.Format("{0:0.00}", repayAmount)), "Repay Amount is wrong.");

        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-247")]
        public void RepayDuePart()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 7;

            //time-shift loan so it's due today
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

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

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayDuePartpaySuccessPage>() as RepayDuePartpaySuccessPage;
            Assert.IsFalse(paymentTakenPage.IsRepayDuePartpaySuccessPageAmountTokenNotPresent());
            

        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-247")]
        public void RepayOverdueFull()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 15;

            //time-shift loan so it's in arrears
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = application.Id, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = application.Id, RepaymentRequestId = requestId2, });

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

        [Test, AUT(AUT.Uk), JIRA("UKWEB-247"), Pending("Fails")]
        public void RepayOverduePart()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 15;

            //time-shift loan so it's in arrears
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);
            
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = application.Id, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = application.Id, RepaymentRequestId = requestId2, });

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

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayOverduePartpaySuccessPage>() as RepayOverduePartpaySuccessPage;
            Assert.IsFalse(paymentTakenPage.IsRepayOverduePartpaySuccessPageAmountTokenNotPresent());
  

        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-247")]
        public void RepayEarlyLessThanMinPayment()
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
            requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString(), "154");

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayEarlyPartpaySuccessPage>() as RepayEarlyPartpaySuccessPage;

            Assert.IsFalse(paymentTakenPage.IsRepayEarlyPartpaySuccessPageAmountTokenNotPresent());
            Assert.IsFalse(paymentTakenPage.IsRepayEarlyPartpaySuccessPageDateTokenNotPresent());

            paymentTakenPage.Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "my-account/repay");
            var requestPage2 = new RepayRequestPage(this.Client);
            requestPage2.WantToRepayBox = "0";
            Assert.AreEqual(Decimal.Parse(requestPage2.OweToday.Remove(0, 1)), Decimal.Parse(requestPage2.WantToRepayBox));
            requestPage2.WantToRepayBox = "5.65";
            Assert.AreEqual(Decimal.Parse(requestPage2.OweToday.Remove(0, 1)), Decimal.Parse(requestPage2.WantToRepayBox));
        }
    }
}