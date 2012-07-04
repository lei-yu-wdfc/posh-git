using MbUnit.Framework;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds.Payments
{
    [TestFixture(Order = 3)]
    public class ServiceDBChangesTest
    {
        private bool _cdcStagingAgentJobWasEnabled;
        private bool _hdsAgentJobWasEnabled;

        [FixtureSetUp]
        [Category("Auto")]
        [Description("This is the text fixture setup for all tests")]
        public void FixtureSetup()
        {
            _cdcStagingAgentJobWasEnabled = HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob);
            _hdsAgentJobWasEnabled = HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob);
        }

        [FixtureTearDown]
        [Category("Auto")]
        [Description("This is the text fixture teardown for all tests")]
        public void FixtureTearDown()
        {
            if (_cdcStagingAgentJobWasEnabled)
            {
                HdsUtilities.EnableJob(HdsUtilities.CdcStagingAgentJob);
            }

            if (_hdsAgentJobWasEnabled)
            {
                HdsUtilities.EnableJob(HdsUtilities.HdsLoadAgentJob);
            }
        }

        [Test]
        [Category("Auto")]
        [Description("Test for new column in ServiceDB -- CDC SSIS Job Fails")]
        public void AddNewColumnInServiceDB_CDCSSISJobFails()
        {

            SMODatabaseAlterations.RemoveAColumn(Drive.Data.NameOfServer, "Payments", "Payment", "Applications", "col5");

            SQLServerAgentJobs.Execute(HdsUtilities.CdcStagingAgentJob);

            Assert.AreEqual(CompletionResult.Succeeded, SQLServerAgentJobs.LastRunOutcome(HdsUtilities.CdcStagingAgentJob));

            SMODatabaseAlterations.AddAColumn(Drive.Data.NameOfServer, "Payments", "Payment", "Applications", "col5", DataType.BigInt);

            SQLServerAgentJobs.Execute(HdsUtilities.CdcStagingAgentJob);

            Assert.AreEqual(CompletionResult.Failed,SQLServerAgentJobs.LastRunOutcome(HdsUtilities.CdcStagingAgentJob));

            SMODatabaseAlterations.RemoveAColumn(Drive.Data.NameOfServer, "Payments", "Payment", "Applications", "col5");
        }

        [Test]
        [Category("Auto")]
        [Description("Test for new column in ServiceDB -- HDS SSIS Job Fails")]
        public void AddNewColumnInCDCStagingDB_HDSSSISJobFails()
        {
            SMODatabaseAlterations.RemoveAColumn(Drive.Data.NameOfHdsServer, HdsUtilities.CDCDatabaseName, "Payment", "Applications", "col5");

            SQLServerAgentJobs.Execute(HdsUtilities.HdsLoadAgentJob);

            Assert.AreEqual(CompletionResult.Succeeded, SQLServerAgentJobs.LastRunOutcome(HdsUtilities.HdsLoadAgentJob));

            SMODatabaseAlterations.AddAColumn(Drive.Data.NameOfHdsServer, HdsUtilities.CDCDatabaseName, "Payment", "Applications", "col5", DataType.BigInt);

            SQLServerAgentJobs.Execute(HdsUtilities.HdsLoadAgentJob);

            Assert.AreEqual(CompletionResult.Failed, SQLServerAgentJobs.LastRunOutcome(HdsUtilities.HdsLoadAgentJob));

            SMODatabaseAlterations.RemoveAColumn(Drive.Data.NameOfHdsServer, HdsUtilities.CDCDatabaseName, "Payment", "Applications", "col5");
       }
    }
}
