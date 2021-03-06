﻿using System.Collections.Generic;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using System.Linq;
using System;

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    class RepayTests : UiTest
    {
        #region Repay Success
        [Test, JIRA("UKWEB-247"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void DefaultRepayPageValuesAreCorrect()
        {
            // Build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
            
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

        [Test, JIRA("UKWEB-247"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void ChangeWantToRepayBox()
        {

            var AmountToRepayMinimum = 5;
            // Build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

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

        [Test, JIRA("UKWEB-247"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void ClickingCancelOnRepayPageOpensMySummaryPage()
        {
            var loginPage = Client.Login();

            // Build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var mySummaryPage = loginPage.LoginAs(email);

            // Open Repay Request page
            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            // Click Cancel button
            requestPage.CancelButtonClick();
        }

        [Test, JIRA("UKWEB-247"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void MovingRepaySliderRemainingAmountShouldBeCorrect()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Runs assertions internally
            requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString(), "50");
        }

        [Test, JIRA("UKWEB-247", "UKWEB-248"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        [Category(TestCategories.CoreTest)]
        public void RepayEarlyFull()
        {
            //build L0 loan
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            Client.Login().LoginAs(email).RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);
            
            Assert.IsNotEmpty(requestPage.RepayCard);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayEarlyFullpaySuccessPage>() as RepayEarlyFullpaySuccessPage;

            Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageAmountTokenNotPresent());
            Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageDateTokenNotPresent());
   
            // Get the content from the Payment Taken Page
            string paymentTakenText = paymentTakenPage.ContentArea();

            var testTitle = "Success! Your balance has been settled in full";
            var testMessage = "Thanks and nice work for repaying early";

            Assert.IsTrue(paymentTakenPage.Content.Text.Contains(testTitle), "Title is missing on Payment Taken page");
            Assert.IsTrue(paymentTakenPage.Content.Text.Contains(testMessage), "Success Message is missing on Payment Taken page");
        }

        [Test, JIRA("UKWEB-247", "UKWEB-248", "UKWEB-913"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
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

        [Test, JIRA("UKWEB-247", "UKWEB-248"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void RepayDueFull()
        {
            //build L0 loan
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 7;

            //time-shift loan so it's due today
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);
            var paymentTakenPage = repayProcessingPage.WaitFor<RepayDueFullpaySuccessPage>() as RepayDueFullpaySuccessPage;

            // Get the content from the Payment Taken Page
            string paymentTakenText = paymentTakenPage.ContentArea();

            var testTitle = "Success! Your balance has been settled in full";
            var testMessage = "Thanks for keeping your promise. We value your custom and hope we can help again in the future.";

            Assert.IsTrue(paymentTakenPage.Content.Text.Contains(testTitle), "Title is incorrect.");
            Assert.IsTrue(paymentTakenText.Contains(testMessage), "Content area text is incorrect.");
        }

        [Test, JIRA("UKWEB-247", "UKWEB-248"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void RepayDuePart()
        {
            //build L0 loan
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 7;

            //time-shift loan so it's due today
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

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

            // Post payment values
            ApiResponse summaryResponse = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });
            var newDueDateAmount = summaryResponse.Values["CurrentLoanRepaymentAmountOnDueDate"].Single();
            string sNewDueDateAmount = String.Format("{0:0.00}", newDueDateAmount);

            // Get the content from the Payment Taken Page
            string paymentTakenText = paymentTakenPage.ContentArea();
            var title = "Success! Your partial payment has gone through";
            var message = "Please repay this remaining balance without delay to avoid incurring further costs, which is the last thing we want to happen";

            Assert.IsTrue(paymentTakenPage.Content.Text.Contains(title), "Message title is incorrect.");
            Assert.IsTrue(paymentTakenText.Contains(message), "Content area text is incorrect.");
            Assert.IsTrue(paymentTakenText.Contains(sNewDueDateAmount), "New Total Repayable is incorrect.");
        }

        [Test, JIRA("UKWEB-247", "UKWEB-248"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void RepayOverdueFull()
        {
            //build L0 loan
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 15;

            //time-shift loan so it's in arrears
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequest() { ApplicationId = application.Id, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequest() { ApplicationId = application.Id, RepaymentRequestId = requestId2, });

            Client.Login().LoginAs(email).RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayOverduePartpaySuccessPage>() as RepayOverduePartpaySuccessPage;
            Assert.IsFalse(paymentTakenPage.IsRepayOverduePartpaySuccessPageAmountTokenNotPresent());

            // Get the content from the Payment Taken Page
            string paymentTakenText = paymentTakenPage.ContentArea();
            //Thank you for resolving  this situation. We can’t promise your Wonga trust rating won’t have been affected, but we may consider helping you again in the future.
            const string testTitle = "Success! Your balance has been settled in full";
            const string testMessage = "Thank you for resolving this situation. We can’t promise your Wonga trust rating won’t have been affected, but we may consider helping you again in the future.";

            Assert.IsTrue(paymentTakenPage.Content.Text.Contains(testTitle), "Title is incorrect.");
            //Assert.IsTrue(paymentTakenPage.Headers.Contains(testTitle), "Title is incorrect.");
            Assert.IsTrue(paymentTakenText.Contains(testMessage), "Content area text is incorrect.");
        }

        [Test, JIRA("UKWEB-247", "UKWEB-248"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void RepayOverduePart()
        {
            //build L0 loan
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 15;

            //time-shift loan so it's in arrears
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);
            
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequest() { ApplicationId = application.Id, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequest() { ApplicationId = application.Id, RepaymentRequestId = requestId2, });

            Client.Login().LoginAs(email).RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Set partial payment amount, test for correct values at same time
            requestPage.IsRepayRequestPageSliderReturningCorrectOverDueValuesOnChange(application.Id.ToString(), "100");

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayOverduePartpaySuccessPage>() as RepayOverduePartpaySuccessPage;
            Assert.IsFalse(paymentTakenPage.IsRepayOverduePartpaySuccessPageAmountTokenNotPresent());

            // Post payment values
            ApiResponse summaryResponse = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });
            var newDueDateAmount = summaryResponse.Values["CurrentLoanAmountDueToday"].Single();
            string sNewDueDateAmount = String.Format("{0:0.00}", newDueDateAmount);

            // Get the content from the Payment Taken Page
            string paymentTakenText = paymentTakenPage.ContentArea();

            var testTitle = "Success! Your partial payment has gone through";
            var testMessage = "Thanks for making a part-payment on your overdue balance. It’s a step in the right direction.";

            Assert.IsTrue(paymentTakenPage.Content.Text.Contains(testTitle), "Title is incorrect.");
            Assert.IsTrue(paymentTakenText.Contains(testMessage), "Content area text is incorrect.");
            Assert.IsTrue(paymentTakenText.Contains(sNewDueDateAmount), "New Total Repayable is wrong.");   
        }

        [Test, JIRA("UKWEB-247"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void RepayEarlyLessThanMinPayment()
        {
            //build L0 loan
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

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

        [Test, JIRA("UKWEB-247"), Pending("Test in development"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        [Row(3)]
        [Row(4)]
        [Row(6)]
        [Row(7)]
        public void CheckRepaymentVsScenarios(int scenarioId)
        {
            //RepaymentVsScenarios(scenarioId);
        }
        #endregion

        #region Repay Decline
        [Test, JIRA("UK-1833", "UKWEB-244", "UKWEB-912"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void RepayEarlyDecline()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var declinedPage = repayProcessingPage.WaitFor<RepayEarlyPaymentFailedPage>() as RepayEarlyPaymentFailedPage;

            // Post payment values
            ApiResponse response = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });
            var dueDateAmount = response.Values["CurrentLoanRepaymentAmountOnDueDate"].Single();

            string sDueDateAmount = String.Format("£{0:0.00}", dueDateAmount);
            var dueDate = Convert.ToDateTime(response.Values["CurrentLoanDueDate"].Single());
            var sDueDate = Date.GetOrdinalDate(dueDate, "d MMM yyyy");

            var declineText = declinedPage.ContentArea;
            var testTitle = "Sorry we couldn't collect your payment";
            var testMessage = String.Format("This means we will still need to collect {0}", sDueDateAmount);

            Assert.IsFalse(declinedPage.IsPaymentFailedAmountNotPresent());
            Assert.IsFalse(declinedPage.IsPaymentFailedDateNotPresent());
            Assert.IsTrue(declinedPage.Header.Contains(testTitle), "Title is incorrect.");
            Assert.IsTrue(declineText.Contains(testMessage), "Content area text is incorrect.");
            Assert.IsTrue(declineText.Contains(sDueDate), "NextDueDate is incorrect");

            //Verify that the links work 
            declinedPage.RepayRetryClick();

            //This verifies the retry repay page link
            requestPage = new RepayRequestPage(this.Client);

            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            repayProcessingPage = new RepayProcessingPage(this.Client);
            declinedPage = repayProcessingPage.WaitFor<RepayEarlyPaymentFailedPage>() as RepayEarlyPaymentFailedPage;

            declinedPage.AddCardClick();
            //This verifies page is add card page - currently the my personal details page
            var myDetailsPage = new MyPersonalDetailsPage(this.Client);
        }

        [Test, JIRA("UK-1833", "UKWEB-244"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
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
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var declinedPage = repayProcessingPage.WaitFor<RepayDuePaymentFailedPage>() as RepayDuePaymentFailedPage;

            // Post payment values
            ApiResponse summaryResponse = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });
            var newDueDateAmount = summaryResponse.Values["CurrentLoanAmountDueToday"].Single();

            string sNewDueDateAmount = String.Format("£{0:0.00}", newDueDateAmount);
            var declineText = declinedPage.ContentArea;

            var testTitle = "We tried unsuccessfully to collect your payment";
            var testMessage = String.Format("Unfortunately your payment was declined by your bank. This means you still owe {0} which is due today", sNewDueDateAmount);

            Assert.IsFalse(declinedPage.IsPaymentFailedAmountNotPresent());
            Assert.IsTrue(declinedPage.Header.Contains(testTitle), "Title is incorrect.");
            Assert.IsTrue(declineText.Contains(testMessage), "Content area text is incorrect.");

            //Verify that the links work 
            declinedPage.RepayRetryClick();

            //This verifies the retry repay page link
            requestPage = new RepayRequestPage(this.Client);

            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            repayProcessingPage = new RepayProcessingPage(this.Client);
            declinedPage = repayProcessingPage.WaitFor<RepayDuePaymentFailedPage>() as RepayDuePaymentFailedPage;

            declinedPage.AddCardClick();
            //This verifies page is add card page - currently the my personal details page
            var myDetailsPage = new MyPersonalDetailsPage(this.Client);
        }

        [Test, JIRA("UK-1833", "UKWEB-244"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
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

            Client.Login().LoginAs(email).RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var declinedPage = repayProcessingPage.WaitFor<RepayOverduePaymentFailedPage>() as RepayOverduePaymentFailedPage;

            // Post payment values
            ApiResponse summaryResponse = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });
            var newDueDateAmount = summaryResponse.Values["CurrentLoanRepaymentAmountOnDueDate"].Single();
            var newDueDateAmountDecimal = decimal.Parse(newDueDateAmount);

            string sNewDueDateAmount = String.Format("£{0:0.00}", newDueDateAmount);

            // Get the content from the Payment Taken Page
            string declineText = declinedPage.ContentArea;

            var testTitle = "We tried unsuccessfully to collect your payment";
            var testMessage = "We urge you to read the suggestions below and try again as soon as possible, to avoid further interest being charged.";

            Assert.IsFalse(declinedPage.IsPaymentFailedAmountNotPresent());
            Assert.IsTrue(declinedPage.Header.Contains(testTitle), "Title is incorrect.");
            Assert.IsTrue(declineText.Contains(testMessage), "Content area text is incorrect.");

            //Verify that the links work 
            declinedPage.RepayRetryClick();

            //This verifies the retry repay page link
            requestPage = new RepayRequestPage(this.Client);

            requestPage.setSecurityCode("888");
            requestPage.SubmitButtonClick();

            repayProcessingPage = new RepayProcessingPage(this.Client);
            declinedPage = repayProcessingPage.WaitFor<RepayOverduePaymentFailedPage>() as RepayOverduePaymentFailedPage;

            declinedPage.AddCardClick();
            //This verifies page is add card page - currently the my personal details page
            var myDetailsPage = new MyPersonalDetailsPage(this.Client);

        }
        #endregion

        #region Repay Error
        [Test, JIRA("UKWEB-247"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void RepayError()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

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
        #endregion
    }
}