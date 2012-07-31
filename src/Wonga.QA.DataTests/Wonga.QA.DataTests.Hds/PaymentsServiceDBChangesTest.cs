﻿using System;
﻿using System.Threading;
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
        private bool _cdcStagingAgentJobWasStarted;
        private bool _hdsAgentJobWasStarted;

        private HdsUtilitiesAgentJob _hdsUtilitiesAgentJob = null;

        [FixtureSetUp]
        [Description("This is the text fixture setup for all tests")]
        public void FixtureSetup()
        {
            _hdsUtilitiesAgentJob = new HdsUtilitiesAgentJob(HdsUtilitiesBase.WongaService.Payments);

            _cdcStagingAgentJobWasStarted = _hdsUtilitiesAgentJob.StopJob(_hdsUtilitiesAgentJob.CdcStagingAgentJob);
            _hdsAgentJobWasStarted = _hdsUtilitiesAgentJob.StopJob(_hdsUtilitiesAgentJob.HdsLoadAgentJob);
        }

        [FixtureTearDown]
        [Description("This is the text fixture teardown for all tests")]
        public void FixtureTearDown()
        {
            if (_cdcStagingAgentJobWasStarted)
            {
                _hdsUtilitiesAgentJob.StartJob(_hdsUtilitiesAgentJob.CdcStagingAgentJob);
            }

            if (_hdsAgentJobWasStarted)
            {
                _hdsUtilitiesAgentJob.StartJob(_hdsUtilitiesAgentJob.HdsLoadAgentJob);
            }
        }

        [Test]
        [Description("Test for new column in ServiceDB -- CDC SSIS Job Fails")]
        public void AddNewColumnInServiceDB_CDCSSISJobFails()
        {
            SMODatabaseAlterations.RemoveAColumn(Drive.Data.Payments.Server, "Payments", "Payment", "Applications", "col5");

            _hdsUtilitiesAgentJob.StartJob(_hdsUtilitiesAgentJob.CdcStagingAgentJob);

            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.CDCStaging);

            _hdsUtilitiesAgentJob.StopJob(_hdsUtilitiesAgentJob.CdcStagingAgentJob);

            SMODatabaseAlterations.AddAColumn(Drive.Data.Payments.Server, "Payments", "Payment", "Applications", "col5", DataType.BigInt);

            _hdsUtilitiesAgentJob.StartJob(_hdsUtilitiesAgentJob.CdcStagingAgentJob, false);

            SMODatabaseAlterations.RemoveAColumn(Drive.Data.Payments.Server, "Payments", "Payment", "Applications", "col5");

            Assert.AreEqual(CompletionResult.Failed, SQLServerAgentJobs.LastRunOutcome(_hdsUtilitiesAgentJob.CdcStagingAgentJob));
        }

        [Test]
        [Description("Test for new column in ServiceDB -- HDS SSIS Job Fails")]
        public void AddNewColumnInCDCStagingDB_HDSSSISJobFails()
        {
            SMODatabaseAlterations.RemoveAColumn(Drive.Data.Hds.Server, _hdsUtilitiesAgentJob.CDCDatabaseName, "Payment", "Applications", "col5");

            _hdsUtilitiesAgentJob.StartJob(_hdsUtilitiesAgentJob.HdsLoadAgentJob);

            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.HDS);

            _hdsUtilitiesAgentJob.StopJob(_hdsUtilitiesAgentJob.HdsLoadAgentJob);

            SMODatabaseAlterations.AddAColumn(Drive.Data.Hds.Server, _hdsUtilitiesAgentJob.CDCDatabaseName, "Payment", "Applications", "col5", DataType.BigInt);

            _hdsUtilitiesAgentJob.StartJob(_hdsUtilitiesAgentJob.HdsLoadAgentJob);

            SMODatabaseAlterations.RemoveAColumn(Drive.Data.Hds.Server, _hdsUtilitiesAgentJob.CDCDatabaseName, "Payment", "Applications", "col5");

            Assert.AreEqual(CompletionResult.Failed, SQLServerAgentJobs.LastRunOutcome(_hdsUtilitiesAgentJob.HdsLoadAgentJob));
        }
    }
}
