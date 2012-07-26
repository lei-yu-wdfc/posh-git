using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class ForgotPassword : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-162"), Pending("Cactcha is steel on page")]
        public void ClicksOnForgotYourPasswordLinkAndEntersValidEmailAndCaptcha()
        {

            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string captcha = "captcha"; // Get captcha text
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).Build();
            var forgotPasswordPage = loginPage.ForgotPasswordClick();
            var homePage = forgotPasswordPage.EnterEmailAndCaptcha(email, captcha);
            Assert.IsTrue(Drive.ThirdParties.ExactTarget.CheckPaymentReminderEmailSent(email));
        }
    }
}
