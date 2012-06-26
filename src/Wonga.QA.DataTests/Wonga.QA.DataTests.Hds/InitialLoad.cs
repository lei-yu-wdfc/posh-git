using System;
using System.Diagnostics;
using MbUnit.Framework;
using Microsoft.SqlServer.Management.Smo.Agent;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds
{
    class InitialLoad
    {
        private bool _cdcStagingAgentJobWasDisabled;
        private bool _hdsAgentJobWasEnabled;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _cdcStagingAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.CdcStagingAgentJob);
            _hdsAgentJobWasEnabled = HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob);            
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
            if (_cdcStagingAgentJobWasDisabled)
            {
                HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob);
            }

            if (_hdsAgentJobWasEnabled)
            {
                HdsUtilities.EnableJob(HdsUtilities.HdsLoadAgentJob);                
            }
        }

        [Test]
        public void RunInitialLoadAndConfirmThatItSucceeds()
        {
            // clear down HDS
            Drive.Data.Hds.Db.Payment.usp_ClearHdsTables();

            // run initial load
            bool jobSuccess = SQLServerAgentJobs.Execute(HdsUtilities.HdsInitialLoadAgentJob);

            // check job succeeds
            Assert.IsTrue(jobSuccess);

        }

    }
}
