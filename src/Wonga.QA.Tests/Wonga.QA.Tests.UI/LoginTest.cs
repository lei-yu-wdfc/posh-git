using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    class LoginTest : UiTest
    {
        [Test, AUT(AUT.Ca), JIRA("QA-160"), SmokeTest]//don`t work yet
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
