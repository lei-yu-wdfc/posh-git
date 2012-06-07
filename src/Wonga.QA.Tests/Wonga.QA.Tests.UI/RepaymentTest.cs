using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    public class RepaymentTest : UiTest
    {
        [Test, AUT(AUT.Za), Pending("Incomplete. Sad times.")]
        public void ZaManualNaedoRepayment()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());

            // Take a loan for 20 days, accept it and go to the Summary page:
            var summaryPage = journey.ApplyForLoan(200, 20)
                                  .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                  .FillAddressDetails()
                                  .FillBankDetails()
                                  .WaitForAcceptedPage()
                                  .FillAcceptedPage()
                                  .GoToMySummaryPage()
                                  .CurrentPage as MySummaryPage;
            
            // Click the "repay" link in My Account:
            var repaymentOptionsPage = summaryPage.RepayClick();

            // Note the balance today:
            //var balanceToday = repaymentOptionsPage.BalanceToday();

            //var manualRepayPage = repayPage.ManualRepaymentButtonClick();
        }

        [Test, AUT(AUT.Za)]
        public void ZaEasyPayRepayment()
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
            var mySummaryPage = loginPage.LoginAs(email);
            var repayPage = mySummaryPage.RepayClick();
            //Thread.Sleep(20000);

            // Open the "How to use easypay" modal popup and check the title is correct - ZA-2587:
            var easyPayHowToUsePopup = repayPage.HowToUseEasyPayLinkClick;

            // Wait until the popup opens:
            Do.Until(() => Client.Driver.FindElement(By.CssSelector("#fancybox-content")).Displayed);

            // Check the popup title:
            Assert.AreEqual("Repay your wonga.com loan with EasyPay", Client.Driver.FindElement(By.CssSelector("h1#repay-your-wonga.com-loan-with-easypay")).Text);

            // Close the popup:
            Client.Driver.FindElement(By.CssSelector("#fancybox-close")).Click();
            // Check the popup title:
            Assert.AreEqual("Repay your wonga.com loan with EasyPay", Client.Driver.FindElement(By.CssSelector("h1#repay-your-wonga.com-loan-with-easypay")).Text);

            // Close the popup:
            Client.Driver.FindElement(By.CssSelector("#fancybox-close")).Click();

            var expectedeasypayno = repayPage.EasypayNumber;
            var popUpPrintPage = repayPage.EasyPayPrintButtonClick();
            var actualString = Do.Until(() => popUpPrintPage.FindElement(By.CssSelector(UiMap.Get.EasypaymentNumberPrintPage.YourEasyPayNumber)).Text);
            Assert.IsTrue(actualString.StartsWith(">>>>>> "));
            Assert.IsTrue(actualString.EndsWith(expectedeasypayno));
        }
    }
}
