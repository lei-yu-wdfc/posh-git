using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.UiTests.Mocked.Web.Uk
{
    [Parallelizable(TestScope.All), SUT(SUT.WIP), AUT(AUT.Uk), JIRA("UK-785", "UK-788", "UK-795", "UKWEB-151", "UKWEB-985")]
    public class MySummaryScenariosTests : UiTest
    {
        //decimal _amountDueAt60InArrears = 0.00m;

        #region Dictionaries

        Dictionary<int, string> introTexts = new Dictionary<int, string> 
	    {
	    {1, "Hi {first name}. Your current trust rating means you can apply for up to £{500} below."},
        {2, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at any time."},
        {3, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at any time."},
        {4, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at any time."},
        {5, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {6, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {7, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {8, "Hi {first name}. We collected your full payment of £{300} today, as promised.Many thanks for repaying our trust in you.You can now request your available credit up to your current Wonga trust rating of £{500}, at anytime. Thanks for using Wonga and we hope we can help again in the future."},
        {9, "Hi {first name}. Your promised repayment of £{456.34}, due first thing today, was declined by your bank."},
        {10, "Hi {first name}. Your repayment of £{456.34}, promised on {date}, was declined by your bank and is now overdue."},
        {11, "Hi {first name}. Your account is now {26} days in arrears."},
        {12, "Hi {first name}. We are disappointed that your account remains overdue and you are now {46} days in arrears."},
        {13, "Hi {first name}. We are disappointed that your account remains overdue and you are now {61} days in arrears."},
        {14, "Hi {first name}."},
        {15, "Hi {first name}."},
        {16, "Hi {first name}."},
        {17, "Hi {first name}."},
        {19, "Hi {first name}."},
        {20, "Hi {first name}. As a new customer you can apply for up to £{400} below."},
        {21, "Hi {first name}."}
	    };

        Dictionary<int, string> tagCloudTexts = new Dictionary<int, string> 
	    {
         {1, ""},
	     {2, "Request Credit\r\nChange Promise Date\r\nView Loan Details\r\nRepay"}, //Change Promise Date should be only a tooltip
         {3, "Request Credit\r\nChange Promise Date\r\nView Loan Details\r\nRepay"},
         {4, "Request Credit\r\nChange Promise Date\r\nView Loan Details\r\nRepay"}, //Change Promise Date is disabled
         {5, "Change Promise Date\r\nView Loan Details\r\nRepay"},
         {6, "Change Promise Date\r\nView Loan Details\r\nRepay"},
         {7, "View Loan Details\r\nRepay\r\nChange Promise Date"}, //Change Promise Date is disabled
         {8, ""}, 
         {9, "Repay\r\nAdd Payment Card\r\nView Loan Details"},
         {10, "Repay\r\nAdd Payment Card"},
         {11, "Repay\r\nSetup Repayment Plan\r\nAdd Payment Card"},
         {12, "Repay\r\nSetup Repayment Plan\r\nAdd Payment Card"},
         {13, "Repay\r\nSetup Repayment Plan\r\nAdd Payment Card"},
         {14, ""},
         {15, ""},
         {16, ""},
         {17, ""},
         {19, ""},
         {20, ""},
         {21, ""},
	    };

        Dictionary<int, string> loanStatusMessages = new Dictionary<int, string> 
	    {
        {1, ""},
	    {2, "If you would like to change your repayment date it's too early to do it just yet, but you can request a new one from {date extensions available}. You can set a handy reminder to do that below. Please bear in mind that you will need to pay any interest and fees up to that point, in order for a new date to be approved."},
        {3, "If you would like to change your promised repayment date you can do so here. Please note you can only extend your promise date a maximum of three times and will need to pay any interest and fees up to that point each time you extend, in order to have any request approved."},
        {4, "Please also remember you have promised to repay £{total to repay 300.00} on {promise date}, when you simply need to ensure the funds are available in the bank account linked to your primary debit card. Changing your promise date isn't possible at this point, so we look forward to collecting your payment and then being of service again in the future."},
        {5, "If you would like to change your repayment date it's too early to do it just yet, but you can request a new one from {date extensions available}. You can set a handy reminder to do it below. Please bear in mind that you will need to pay any interest and fees up to that point, in order to have your request for a change of promise date approved."},
        {6, "If you would like to change your promised repayment date you can do so here. Please note you can only extend your promise date a maximum of three times and will need to pay any interest and fees up to that point each time you extend, in order to have any request approved."},
        {7, "Changing your promise date isn't possible at this point, so we look forward to collecting your full payment and then being of service again in the future. Thanks for using Wonga!"},
        {9, "We understand genuine mistakes happen so we hope you can make this payment today and save yourself further costs. If the balance isn't cleared by 5pm today, however, you will incur a missed payment fee of £20, which is the last thing we want to happen! Please click repay now to settle your balance. You can add a new debit card if you need to. If you are unable to pay in full today, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {10, "You have unfortunately incurred a missed payment fee of £20 and interest continues to accrue. Please click repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. If you are unable to pay in full today, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {11, "Please act now to avoid incurring further interest, which continues to accrue. Please click repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of four months. If this doesn't work for you, you should call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {12, "Please act now to avoid incurring further interest, which continues to accrue, and potential negative entries on your credit file. Please click repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of six months. If this doesn't work for you, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {13, "Please take action today to avoid incurring further interest, which continues to accrue, and potential negative entries on your credit file. Click Repay now to settle your balance and bring your account back into line. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of six months. If this doesn't work for you, please call our friendly collections team straight away on 0844 842 9109. If you choose not to deal with this matter immediately, we may need to take more formal steps to recover the balance owed."},
        {14, "You have an active repayment plan\r\nTo make an early repayment or discuss your plan please contact customer services on the number displayed on our contact us page."},
        {15, "You have an active repayment plan\r\nTo make an early repayment or discuss your plan please contact customer services on the number displayed on our contact us page."},
        {16, "You have an active repayment plan\r\nTo make an early repayment or discuss your plan please contact customer services on the number displayed on our contact us page."},
        {17, "Your application is in the final stages of our approval process. We hate to keep you waiting, but, on this rare occasion, we need to check a few more details. There's no need to contact us or do anything and you should hear back from us {within the next 6 hours}. You can also check for updates about your application by logging into your account. As soon as we complete our checks, we will email you and send you a text message, so thanks for your patience in the meantime.\nIf approved you will just need to come back to the site and click the ‘I accept’ button on your agreement and we will then send the money to your bank within 15 minutes."},
        {19, "You informed us that you wanted to cancel your credit agreement please contact us on {CS tel. No} to complete this process by making the required repayment."},
        {21, "One last step to receive your cash.\n\nYour application has been approved! Now you just need to read and accept your new agreement and the loan conditions by clicking the ‘I accept’ button in the agreement below. You will then receive {£loan amount} in your account.\n\nWe’ll then collect {£xx.xx total repayable on due date} from your debit card on {repayment date in format 15th March 2011.}\n\nThanks for using Wonga!"},
	    };

        Dictionary<int, string> PromiseSummaryTexts = new Dictionary<int, string> 
	    {
	    {1, ""},
        {2, "I promised to pay {£245} in {10} days ({10th May 2012})"},
        {3, "I promised to pay {£245} in {10} days ({10th May 2012})"},
        {4, "I promised to pay {£245} in {10} days ({10th May 2012})"},
        {5, "I promised to pay {£245} in {10} days ({10th May 2012})"},
        {6, "I promised to pay {£245} in {10} days ({10th May 2012})"},
        {7, "I promised to pay {£245} in {10} days ({10th May 2012})"},
        {8, ""},
        {9, "I promised to pay {£456.34} today"},
        {10, "I owe {£456.34} today"},
        {11, "I owe {£456.34} today"},
        {12, "I owe {£456.34} today"},
        {13, "I owe {£456.34} today"},
        {14, ""},
        {15, ""},
        {16, ""},
        {17, ""},
        {19, ""},
        {20, ""},
        {21, ""}
	    };

        Dictionary<int, string> sliderType = new Dictionary<int, string> 
	    {
	    {1, "full"},
        {2, "half"},
        {3, "half"},
        {4, "half"},
        {5, "no"},
        {6, "no"},
        {7, "no"},
        {8, "full"},
        {9, "no"},
        {10, "no"},
        {11, "no"},
        {12, "no"},
        {13, "no"},
        {14, "no"},
        {15, "no"},
        {16, "no"},
        {17, "no"},
        {19, "no"},
        {20, "full"},
        {21, "no"}
	    };

        #endregion

        #region Tests

        // Check the my Summary page after we click My Summary buton
        [Test, JIRA("UKWEB-953"), Owner(Owner.StanDesyatnikov)]
        public void ClickMySummaryButton()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();

            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
        }

        // One live drawdown -can request credit, too early to extend
        [Test, MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void MySummaryScenario02()
        {
            const int scenarioId = 2;
            const decimal expectedDueDateBalance = 115.91m;
            var expectedDueDate = new DateTime(2012, 07, 14);

            MySummaryScenarios(scenarioId, expectedDueDateBalance, expectedDueDate);
        }

        #endregion

        #region Helpers

        private void MySummaryScenarios(int scenarioId, decimal expectedDueDateBalance, DateTime expectedDueDate)
        {
            string email = Get.RandomEmail();
            var mySummaryPage = Client.Login().LoginAs(email);

            var helpers = new MockHelpers();
            helpers.SelectMockedScenario(this.Client, "Scenario"+scenarioId.ToString("#"));

            CheckPromiseSummaryText(scenarioId, mySummaryPage, expectedDueDateBalance, expectedDueDate);
        }

        /// <summary>
        /// Checks Promise Summary Text on My Summary page
        /// </summary>
        /// <param name="scenarioId"></param>
        /// <param name="mySummaryPage"></param>
        /// <param name="expectedDueDateBalance"></param>
        /// <param name="expectedDueDate"></param>
        private void CheckPromiseSummaryText(int scenarioId, MySummaryPage mySummaryPage, decimal expectedDueDateBalance, DateTime expectedDueDate)
        {

            int expectedDaysTillDueDate = (expectedDueDate - DateTime.Today.Date).Days;
            expectedDaysTillDueDate = expectedDaysTillDueDate < 0 ? -expectedDaysTillDueDate : expectedDaysTillDueDate;

            if (PromiseSummaryTexts[scenarioId].Length == 0)
            {
                Assert.IsFalse(mySummaryPage.IsPromiseSummaryAvailable(), "Promise Summary should not be available");
            }
            else
            {
                string expectedPromiseSummaryText = PromiseSummaryTexts[scenarioId]
                                                    .Replace("{£245}", "£" + String.Format("{0:0.00}", expectedDueDateBalance))
                                                    .Replace("in {10}", "in " + expectedDaysTillDueDate.ToString("#"))
                                                    .Replace("{10th May 2012}", Date.GetOrdinalDate(expectedDueDate, "ddd d MMM yyyy"))
                                                    .Replace("{£456.34}", "£" + String.Format("{0:0.00}", expectedDueDateBalance));
                if ((scenarioId == 7) && (expectedDueDate.Equals(DateTime.Today)))
                {
                    expectedPromiseSummaryText = "I promised to pay {£245} today ({10th May 2012})"
                                                  .Replace("{£245}", "£" + String.Format("{0:0.00}", expectedDueDateBalance))
                                                  .Replace("{10th May 2012}", Date.GetOrdinalDate(expectedDueDate, "ddd d MMM yyyy"));
                }
                Assert.AreEqual(expectedPromiseSummaryText, mySummaryPage.GetPromiseSummaryText);
            }

        }
        #endregion
    }

}