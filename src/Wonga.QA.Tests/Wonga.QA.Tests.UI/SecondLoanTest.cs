using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;


namespace Wonga.QA.Tests.Ui
{
    class SecondLoanTest : UiTest
    {
        [Test, AUT(AUT.Za), JIRA("QA-195"), Pending("need refinement")]
        public void InformationAboutSecondLoanShouldBeDisplayedForZa()
        {
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.RepayOnDueDate();
            application = ApplicationBuilder.New(customer).Build();
            // Check data in DB
           Assert.AreEqual("649.89", application.GetBalance().ToString(CultureInfo.InvariantCulture));
           // Check on my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
            Assert.AreEqual("R649.89", mySummaryPage.GetTotalToRepay);
            // Check data in SF
            var salesForceStartPage = Client.SalesForceStart();
            string employeeName = Config.SalesforceUi.Username;
            string employeePassword = Config.SalesforceUi.Password;
            var salesForceHome = salesForceStartPage.LoginAs(employeeName, employeePassword);
            var salesForceSearchResultPage = salesForceHome.FindCustomerByMail(email);
            Thread.Sleep(2000);
            if (salesForceSearchResultPage.IsCustomerFind())
            {
                salesForceSearchResultPage.GoToCustomerDetailsPage();
               
            }
        }
    }
}
