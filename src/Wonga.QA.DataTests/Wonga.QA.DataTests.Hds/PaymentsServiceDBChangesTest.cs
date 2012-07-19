﻿using MbUnit.Framework;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
﻿using Wonga.QA.DataTests.Hds.Common;
﻿using Wonga.QA.Framework;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds.Payments
{
    [TestFixture(Order = 3)]
    [Category("Auto")]
    [Category("Payments")]
    public class PaymentsServiceDBChangesTest
    {
        private bool _cdcStagingAgentJobWasEnabled;
        private bool _hdsAgentJobWasEnabled;

        private HdsUtilities _hdsUtilities = null;

        [FixtureSetUp]
        [Description("This is the text fixture setup for all tests")]
        public void FixtureSetup()
        {
            _hdsUtilities = new HdsUtilities(HdsUtilities.WongaService.Payments);

            _cdcStagingAgentJobWasEnabled = _hdsUtilities.DisableJob(_hdsUtilities.CdcStagingAgentJob);
            _hdsAgentJobWasEnabled = _hdsUtilities.DisableJob(_hdsUtilities.HdsLoadAgentJob);
        }

        [FixtureTearDown]
        [Description("This is the text fixture teardown for all tests")]
        public void FixtureTearDown()
        {
            if (_cdcStagingAgentJobWasEnabled)
            {
                _hdsUtilities.EnableJob(_hdsUtilities.CdcStagingAgentJob);
            }

            if (_hdsAgentJobWasEnabled)
            {
                _hdsUtilities.EnableJob(_hdsUtilities.HdsLoadAgentJob);
            }
        }

        [Test]
        [Description("Test for new column in ServiceDB -- CDC SSIS Job Fails")]
        public void AddNewColumnInServiceDB_CDCSSISJobFails()
        {

            SMODatabaseAlterations.RemoveAColumn(Drive.Data.Payments.Server, "Payments", "Payment", "Applications", "col5");

            SQLServerAgentJobs.Execute(_hdsUtilities.CdcStagingAgentJob);

            Assert.AreEqual(CompletionResult.Succeeded, SQLServerAgentJobs.LastRunOutcome(_hdsUtilities.CdcStagingAgentJob));

            SMODatabaseAlterations.AddAColumn(Drive.Data.Payments.Server, "Payments", "Payment", "Applications", "col5", DataType.BigInt);

            SQLServerAgentJobs.Execute(_hdsUtilities.CdcStagingAgentJob);

            Assert.AreEqual(CompletionResult.Failed, SQLServerAgentJobs.LastRunOutcome(_hdsUtilities.CdcStagingAgentJob));

            SMODatabaseAlterations.RemoveAColumn(Drive.Data.Payments.Server, "Payments", "Payment", "Applications", "col5");
        }

        [Test]
        [Description("Test for new column in ServiceDB -- HDS SSIS Job Fails")]
        public void AddNewColumnInCDCStagingDB_HDSSSISJobFails()
        {
            SMODatabaseAlterations.RemoveAColumn(Drive.Data.Hds.Server, _hdsUtilities.CDCDatabaseName, "Payment", "Applications", "col5");

            SQLServerAgentJobs.Execute(_hdsUtilities.HdsLoadAgentJob);

            Assert.AreEqual(CompletionResult.Succeeded, SQLServerAgentJobs.LastRunOutcome(_hdsUtilities.HdsLoadAgentJob));

            SMODatabaseAlterations.AddAColumn(Drive.Data.Hds.Server, _hdsUtilities.CDCDatabaseName, "Payment", "Applications", "col5", DataType.BigInt);

            SQLServerAgentJobs.Execute(_hdsUtilities.HdsLoadAgentJob);

            Assert.AreEqual(CompletionResult.Failed, SQLServerAgentJobs.LastRunOutcome(_hdsUtilities.HdsLoadAgentJob));

            SMODatabaseAlterations.RemoveAColumn(Drive.Data.Hds.Server, _hdsUtilities.CDCDatabaseName, "Payment", "Applications", "col5");
        }
    }
}
