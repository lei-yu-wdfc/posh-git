using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
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
