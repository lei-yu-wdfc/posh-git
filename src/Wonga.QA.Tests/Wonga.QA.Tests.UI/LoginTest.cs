using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    class LoginTest : UiTest
    {
        [Test, AUT(AUT.Ca), JIRA("QA-160")]
        public void LogInShouldBeRedirectedToMySummaryPage()
        {
            var homePage = Client.Home();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            var page = homePage.Login.LoginAs(email, "Passw0rd");

        }
    }
}
