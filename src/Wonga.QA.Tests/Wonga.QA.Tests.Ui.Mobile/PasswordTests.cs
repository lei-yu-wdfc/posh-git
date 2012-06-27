using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Ui.Pages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Mobile
{
    public class PasswordTests : UiMobileTest
    {
        [Test, AUT(AUT.Za)]
        public void UpdatePassword()
        {
            Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "login");
            var login = Do.Until(() => new LoginPageMobile(Client));
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New()
                                        .WithEmailAddress(email)
                                        .Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.RepayOnDueDate();
            var summaryPage = login.LoginAs(email, Get.GetPassword());
            var myPersonalDetailsPage = summaryPage.GoToMyPersonalDetailsPage();
            myPersonalDetailsPage.EditPassword(Get.GetPassword(), "Newpassw0rd");
        }
    }
}
