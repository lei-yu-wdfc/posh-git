using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;

using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Ui;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.Tests.Migration
{
    public class MigrationLoginTests:UiTest
    {

        [Test]
        // Migrated V2 customer tries to log in into V2 and is redirected to V3.
        public void MigratedV2CustomerLogsInToV2RedirectedToV3Test()
        {
            // TBD: create a login to V2 environment QAF method
            // 1. Log in into V2
            // 2. Check that V3 My Summary page is open.

        }

        [Test]
        // Migrated V2 customer tries to log in into V3 successfully.
        public void MigratedV2CustomerLogsInToV3Test()
        {
            //var test = v2db;
            //var accountsTab = Drive.Data.Ops.Db.Accounts;

            //Client.Login().LoginAs("qa.wonga.com+QB-WK-158-d540d574-0c66-4f42-b02a-aec5a9d2bde4@gmail.com","Passw0rd");
            
            //var wongaWholeStaging = Drive.Data.WongaWholeStaging.Db.greyface.Users;
            //var userPassword = wongaWholeStaging.Find(wongaWholeStaging.user_name == "claire_coe@lycos.co.uk").password;


            var migHelper = new MigrationHelper();
            
            var migratedAccountLogin = migHelper.GetMigratedAccountLogin();
            var migratedAccountLoginPassword = migHelper.GetMigratedAccountLoginPassword(migratedAccountLogin);

            // Likely we don't need to assert here because if mySummary object is not created successfully, an excption will be thrown
        }


        [Test]
        // Not-migrated V2 customer logs in into V2 successfully.
        public void NonMigratedV2CustomerLogsInToV2Test()
        {
            // 1. Log in into V2
            // 2. Check that V2 My Summary page is open.
        }

        [Test]
        // Not-migrated V2 customer tries to log in into V3 and is redirected to V2.
        public void NonMigratedV2CustomerLogsInToV3RedirectedToV2Test()
        {
            // 1. Log in into V3
            // 2. Check that V2 My Summary page is open.
        }

        [Test]
        public void CreateUser()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();//"testMigratedUser@gmail.com";

            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();

            Application application = ApplicationBuilder
                .New(customer)
                .Build();

            loginPage.LoginAs(email);
            
        }

        [Test]
        public void CreateUserFromWeb()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email);
            var mySummary = journeyL0.Teleport<MySummaryPage>() as MySummaryPage;
        }

    }
}
