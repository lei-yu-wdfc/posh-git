using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;
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

        [Test, AUT(AUT.Za)]
        public void UpdateHomeTelephoneNumber()
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
            //edit home phone number
            const string number = "0213456789";
            var refreshedPersonalDetailsPage = myPersonalDetailsPage.EditHomeTelephoneNumber(number);
            Assert.IsTrue(refreshedPersonalDetailsPage.Phone.Text.Contains(number));
        }

        [Test, AUT(AUT.Za)]
        public void UpdateMobileTelephoneNumber()
        {
            var login = Client.Login();
            //string email = Get.RandomEmail();
            //Customer customer = CustomerBuilder.New()
            //    .WithEmailAddress(email)
            //    .Build();
            //Application application = ApplicationBuilder.New(customer).Build();
            //application.RepayOnDueDate();
            var custhelp = VanillaCustomerHelper.New();
            string email = custhelp.GetVanillaCustomer().Email;
            
            var summaryPage = login.LoginAs(email, Get.GetPassword());
            var myPersonalDetailsPage = summaryPage.GoToMyPersonalDetailsPage();
            //edit mobile number
            const string number = "0210006789";
            var refreshedPersonalDetailsPage = myPersonalDetailsPage.EditMobileTelephoneNumber(number);
            Assert.IsTrue(refreshedPersonalDetailsPage.Phone.Text.Contains(number));
        }

        [Test, AUT(AUT.Za)]
        public void TestUserCreation()
        {
            //for (int i = 0; i < 50; i++)
            //{
            //    var c = new VanillaCustomer();
            //    c.InsertUserToDb();
            //}


            //var x = new VanillaCustomerHelper();
            //var y = x.GetVanillaCustomer().Email;
            //Assert.IsTrue(y.Email.Contains("gmail"));
            //Assert.That(y.IsUsed, Is.EqualTo(1));

            //var z = VanillaCustomerHelper.New();
            //var y = z.GetVanillaCustomer();

            //Assert.IsTrue(y.Email.Contains("gmail"));
            //Assert.That(y.IsUsed, Is.EqualTo(1));
            var nystrings = new string[3];
            foreach (var nystring in nystrings)
            {
                Console.WriteLine(nystring);
            }

        }
    }
}
