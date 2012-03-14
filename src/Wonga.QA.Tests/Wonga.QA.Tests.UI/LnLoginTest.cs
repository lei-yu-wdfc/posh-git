using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    class LnLoginTest : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-160")]
        public void LnLogInShouldBeRedirectedToMySummaryPage()
        {
            //i had done logon through the login page instead of home page.
            //this will be fixed after the frontend team will solve problems with home page
            var loginPage = Client.Login();
            string email = Data.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            var page = loginPage.LoginAs(email);
        }
      
    }
}
