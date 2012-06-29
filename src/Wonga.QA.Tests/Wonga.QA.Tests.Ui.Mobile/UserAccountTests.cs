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
    public class UserAccountTests : UiMobileTest
    {
        [Test, AUT(AUT.Za)]
        public void UpdatePassword()
        {
            var login = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New()
                .WithEmailAddress(email)
                .Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.RepayOnDueDate();
            var summaryPage = login.LoginAs(email, Get.GetPassword());
            var myPersonalDetailsPage = summaryPage.GoToMyPersonalDetailsPage();
            var refreshedMyPersonalDetailsPage = myPersonalDetailsPage.EditPassword(Get.GetPassword(), "Newpassw0rd");

            //logout and login with new password
            var homePage = refreshedMyPersonalDetailsPage.TabsElementMobile.LogOut();
            var newSummaryPage = homePage.Tabs.LogIn(email, "Newpassw0rd");
        }

        [Test, AUT(AUT.Za)]
        public void UpdateAddressDetails()
        {
            var login = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New()
                .WithEmailAddress(email)
                .Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.RepayOnDueDate();
            var summaryPage = login.LoginAs(email, Get.GetPassword());
            var myPersonalDetailsPage = summaryPage.GoToMyPersonalDetailsPage();
            myPersonalDetailsPage.EditAddress();
        }
    }
}
