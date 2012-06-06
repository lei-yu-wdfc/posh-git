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

        [Test, AUT(AUT.Za), Pending("Code not yet on rc.za.wonga.com as of 24/05/12")]
        public void ZaEasyPayRepayment()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var summaryPage = journey.ApplyForLoan(200, 10)
                              .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                              .FillAddressDetails()
                              .FillAccountDetails()
                              .FillBankDetails()
                              .WaitForAcceptedPage()
                              .FillAcceptedPage()
                              .GoToMySummaryPage()
                              .CurrentPage as MySummaryPage;
            var repayPage = summaryPage.RepayClick();
            var expectedeasypayno = repayPage.EasypayNumber;
            var popUpPrintPage = repayPage.EasyPayPrintButtonClick();
            var actualString = Do.Until(() => popUpPrintPage.FindElement(By.CssSelector(UiMap.Get.EasypaymentNumberPrintPage.YourEasyPayNumber)).Text);
            Assert.IsTrue(actualString.StartsWith(">>>>>> "));
            Assert.IsTrue(actualString.EndsWith(expectedeasypayno));
        }
    }
}
