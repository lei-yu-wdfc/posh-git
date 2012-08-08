using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    class LoginTest : UiTest
    {
        [Test, JIRA("QA-160"), Category(TestCategories.SmokeTest)]//don`t work yet
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
