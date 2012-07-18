using System;
using System.Diagnostics;
using MbUnit.Framework;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds.Common
{
    public class Reconciliation
    {
        /// <summary>
        /// Run the reconcilliation and confirm that it succeeds
        /// </summary>
        /// <param name="wongaService">Service to run for</param>
        public void RunReconciliationAndConfirmThatItSucceeds(HdsUtilities.WongaService wongaService)
        {
            HdsUtilities hdsUtilities = new HdsUtilities(wongaService);

            SQLServerAgentJobs.WaitUntilJobComplete(hdsUtilities.CdcStagingAgentJob);
            SQLServerAgentJobs.WaitUntilJobComplete(hdsUtilities.HdsLoadAgentJob);

            bool jobSuccess = SQLServerAgentJobs.Execute(hdsUtilities.HdsReconcileAgentJob);

            // check job succeeds
            Assert.IsTrue(jobSuccess);
        }
    }
}
