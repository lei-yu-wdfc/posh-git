using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.Tests.Ui
{
    class MySummary : UiTest
    {

        [Test, AUT(AUT.Ca)]
        public void IsRepaymentWarningAvailableForLn()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            CustomerBuilder.New().WithEmailAddress(email).Build();
            var summary = loginPage.LoginAs(email);

            summary.Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "/repay-canada");
            var xpath = summary.Client.Driver.FindElement(By.XPath("//div[@id='content-area']/p[1]"));
            Assert.IsTrue(xpath.Text.Contains("via online banking"));
        }

        [Test, AUT(AUT.Ca)]
        public void ThisIsTestSoBuggerOff()
        {
            
        }


    }
}
