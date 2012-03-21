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
