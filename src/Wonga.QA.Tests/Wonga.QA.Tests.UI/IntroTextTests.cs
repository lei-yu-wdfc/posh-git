using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using EmploymentStatusEnum = Wonga.QA.Framework.Msmq.EmploymentStatusEnum;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.Db.Extensions;


namespace Wonga.QA.Tests.Ui
{
    public class IntroTextTests: UiTest
    {
        Dictionary<int, string> introTexts = new Dictionary<int, string> 
	    {
	    {1, "Hi {first name}. Your current trust rating means you can apply for up to £{500} below."},
        {2, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at anytime."},
        {3, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at anytime."},
        {4, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at anytime."},
        {5, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {6, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {7, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {8, "Hi {first name}. We collected your full payment of £{300} today, as promised.Many thanks for repaying our trust in you.You can now request your available credit up to your current Wonga trust rating of £{500}, at anytime. Thanks for using Wonga and we hope we can help again in the future."},
        {9, "Hi {first name}. Your promised repayment of £{456.34}, due first thing today, was declined by your bank."},
        {10, "Hi {first name}. Your repayment of £{456.34}, promised on {date}, was declined by your bank and is now overdue."},
        {11, "Hi {first name}. Your account is now {26} days in arrears."},
        {12, "Hi {first name}. We are dissapointed that your account remains overdue and you are now {46} days in arrears."},
        {13, "Hi {first name}. We are dissapointed that your account remains overdue and you are now {61} days in arrears."},
        {14, "Hi {first name}. We are dissapointed that your account remains overdue and you are now {61} days in arrears."},
        {15, "Hi {first name}. We were unable to collect a scheduled repayment towards your current repayment plan."},
        {16, "Hi {first name}. You have missed a scheduled payment against your agreed repayment plan, which has now been cancelled."},
        {17, "Hi {first name}."},
        {19, "Hi {first name}."},
        {20, "Hi {first name}. As a new customer you can apply for up to £{400} below."},
        {21, "Hi {first name}."}
	    };

        [Test, AUT(AUT.Uk), JIRA("UK-788", "UK-1614")]
        public void IntroTextScenario1A()
        {
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var journey = JourneyFactory.GetL0Journey(Client.Home());          
            var aPage = journey.ApplyForLoan(loanAmount, days)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails()
                .FillAccountDetails();
 
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
                
            // Check the actual text
            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedlntroText = introTexts[1];
            expectedlntroText = expectedlntroText.Replace("{first name}", journey.FirstName).Replace("{500}", "400"); 
            Assert.AreEqual(expectedlntroText, actuallntroText);
        }
    }
}