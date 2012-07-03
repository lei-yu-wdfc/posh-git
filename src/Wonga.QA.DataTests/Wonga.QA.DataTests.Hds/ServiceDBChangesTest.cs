using MbUnit.Framework;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds.Payments
{
    [TestFixture]
    public class ServiceDBChangesTest
    {
        [Test]
        [AUT(AUT.Uk)]
        [Description("Test for new column in ServiceDB -- SSIS Job Fails")]
        [Category("Auto")]
        public void AddNewColumnInServiceDB_CDCSSISJobFails()
        {
            SMODatabaseAlterations.RemoveAColumn(Drive.Data.NameOfServer, "Payments", "Payment", "Applications", "col5");

            bool cdcStagingAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.CdcStagingAgentJob);

            HdsUtilities.WaitUntilJobRun(HdsUtilities.CdcStagingAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.CdcStagingAgentJob);

            Assert.AreEqual(CompletionResult.Succeeded,
                            SQLServerAgentJobs.LastRunOutcome(HdsUtilities.CdcStagingAgentJob));

            SMODatabaseAlterations.AddAColumn(Drive.Data.NameOfServer, "Payments", "Payment", "Applications", "col5", DataType.BigInt);
           //
            // Start the CDC Staging load job and check data once finished
            //
            HdsUtilities.WaitUntilJobRun(HdsUtilities.CdcStagingAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.CdcStagingAgentJob);



            Assert.AreEqual(CompletionResult.Failed,SQLServerAgentJobs.LastRunOutcome(HdsUtilities.CdcStagingAgentJob));

            SMODatabaseAlterations.RemoveAColumn(Drive.Data.NameOfServer, "Payments", "Payment", "Applications", "col5");

            //
            // Start the HDS load job and check data once finished
            //

            // reset jobs to original state
            if (cdcStagingAgentJobWasDisabled)
            { HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob); }
        }

        [Test]
        [AUT(AUT.Uk)]
        [Description("Test for new column in ServiceDB -- SSIS Job Fails")]
        [Category("Auto")]
        public void AddNewColumnInCDCStagingDB_HDSSSISJobFails()
        {
            SMODatabaseAlterations.RemoveAColumn(Drive.Data.NameOfHdsServer, HdsUtilities.CDCDatabaseName, "Payment", "Applications", "col5");

            bool hdsStagingAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.HdsLoadAgentJob);

            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsLoadAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.HdsLoadAgentJob);

            Assert.AreEqual(CompletionResult.Succeeded,
                            SQLServerAgentJobs.LastRunOutcome(HdsUtilities.HdsLoadAgentJob));

            SMODatabaseAlterations.AddAColumn(Drive.Data.NameOfHdsServer, HdsUtilities.CDCDatabaseName, "Payment", "Applications", "col5", DataType.BigInt);
            //
            // Start the CDC Staging load job and check data once finished
            //
            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsLoadAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.HdsLoadAgentJob);

            Assert.AreEqual(CompletionResult.Failed, SQLServerAgentJobs.LastRunOutcome(HdsUtilities.HdsLoadAgentJob));

            SMODatabaseAlterations.RemoveAColumn(Drive.Data.NameOfHdsServer, HdsUtilities.CDCDatabaseName, "Payment", "Applications", "col5");

            //
            // Start the HDS load job and check data once finished
            //

            // reset jobs to original state
            if (hdsStagingAgentJobWasDisabled)
            { HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob); }
        }

    }
}
