using MbUnit.Framework;
using Wonga.QA.Framework;
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
            var accountsTab = Drive.Data.Ops.Db.Accounts;
            
            Client.Login().LoginAs("claire_coe@lycos.co.uk", "kieran14");
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

    }
}
