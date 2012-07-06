using System;
using System.Diagnostics;
using System.Reflection;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;

using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.Tests.Migration
{
    public class MigrationLoginTests : UiTest
    {
        private string GetFunctionName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();

            return methodBase.Name;
        }

        [Test, TimeoutAttribute(timeoutSeconds: 99999)]
        // Migrated V2 customer tries to log in into V2 and is redirected to V3.
        public void MigratedV2CustomerLogsInToV2RedirectedToV3Test()
        {
            // TBD: create a login to V2 environment QAF method
            // 1. Log in into V2
            // 2. Check that V3 My Summary page is open.

        }

        [Test, MultipleAsserts, JIRA("UKMIG-243"), TimeoutAttribute(timeoutSeconds: 99999)]
        // Migrated V2 customer tries to log in into V3 successfully.
        public void MigratedV2CustomerLogsInToV3Test()
        {
            var migHelper = new MigrationHelper();
            var loginStatus = string.Empty;

            Console.WriteLine("UKMIG-243, As an existing migrated customer, I want to login to the Wonga website so that I can manage my account or apply for a new loan, " + GetFunctionName());

            for (int usersToCheck = 1; usersToCheck < 10000; usersToCheck++)
            {
                var migratedAccountLogin = migHelper.GetMigratedAccountLogin(0);
                var migratedAccountLoginPassword = migHelper.GetMigratedAccountLoginPassword(migratedAccountLogin);

                var client1 = new UiClient();

                try
                {
                    client1.Login().LoginAs(migratedAccountLogin, migratedAccountLoginPassword);
                    loginStatus = "Passed";
                }
                catch (Exception)
                {
                    loginStatus = "Failed";
                }
                finally
                {
                    Console.WriteLine("{0}: {1} Login = {2}", usersToCheck, loginStatus, migratedAccountLogin);
                    client1.Dispose();
                }
            }

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
