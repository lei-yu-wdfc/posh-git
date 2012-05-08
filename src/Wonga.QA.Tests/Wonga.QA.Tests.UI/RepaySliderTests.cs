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
    class RepaySliderTests : UiTest
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
            requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString());

            //Branch point - Add Cv2 for each path and proceed
            //requestPage.setSecurityCode("888");
            //requestPage.SubmitButtonClick();

            //var extensionProcessingPage = new ExtensionProcessingPage(this.Client);

            //var declinedPage = extensionProcessingPage.WaitFor<ExtensionPaymentFailedPage>() as ExtensionPaymentFailedPage;

            //Assert.IsFalse(declinedPage.IsPaymentFailedAmountNotPresent());
            //Assert.IsFalse(declinedPage.IsPaymentFailedDateNotPresent());
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
            var expectedOweToday = application.GetBalance();
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

        /*[Test, AUT(AUT.Uk), JIRA("UK-1827"), Pending("In development")]
        [Row()]
        public void ChangeWantToRepayBoxTest(int anountToRepay)
        {
            ChangeWantToRepayBox(anountToRepay);
        }*/

        [Test, AUT(AUT.Uk), JIRA("UK-1827"), Pending("In development")]
        public void ChangeWantToRepayBox()
        {
            // Build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            
            // TBD
            // add conditional extra steps to:
            // set Today = Next Due Date
            // set Next Due Date is in arears

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            // Open Repay Request page
            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            

            // You currently owe
            var expectedOweToday = application.GetBalance();
            string sExpectedOweToday = String.Format("{0:0.00}", expectedOweToday);

            // TBD - change values in the Want to Repay box
            var amountToRepayList = new List<decimal> { 1, Convert.ToInt16(expectedOweToday - 1), expectedOweToday };
            var random = new Random();
            amountToRepayList.Add(random.Next(1, Convert.ToInt16(expectedOweToday - 2)));
            
            string sActualOweToday;
           // decimal expectedWantToRepay;
            string sExpectedWantToRepay;
            //string sActualWantToRepay;

            foreach (decimal amountToRepay in amountToRepayList)
            {
                requestPage.WantToRepayBox = amountToRepay.ToString("#.##");
                Thread.Sleep(2000);

                // Currently Owe
                sActualOweToday = requestPage.OweToday.TrimStart('£');
                Assert.AreEqual(sExpectedOweToday, sActualOweToday, "Currently Owe is wrong.");

                // Remainder to repay = Amount Owed - Repay Amout
                var expectedReminderToRepay = expectedOweToday - amountToRepay;
                string sExpectedReminderToRepay = String.Format("{0:0.00}", expectedReminderToRepay);
                string sActualReminderToRepay = requestPage.RemainderAmount.TrimStart('£');
                Assert.AreEqual(sExpectedReminderToRepay, sActualReminderToRepay, "Reminder Amount is wrong.");

                // Repay Total in the Read Me message
                sExpectedWantToRepay = String.Format("{0:0.00}", amountToRepay);
                string sActualRepayTotal = requestPage.RepayTotal.TrimStart('£');
                Assert.AreEqual(sExpectedWantToRepay, sActualRepayTotal, "Repay Total in the Read Me message is wrong.");    
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


    }
}