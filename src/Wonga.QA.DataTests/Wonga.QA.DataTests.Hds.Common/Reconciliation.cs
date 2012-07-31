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
        public void RunReconciliationAndConfirmThatItSucceeds(HdsUtilitiesBase.WongaService wongaService)
        {
            HdsUtilitiesAgentJob hdsUtilitiesAgentJob = new HdsUtilitiesAgentJob(wongaService);

            bool jobSuccess = SQLServerAgentJobs.Execute(hdsUtilitiesAgentJob.HdsReconcileAgentJob);

            // check job succeeds
            Assert.IsTrue(jobSuccess);
        }
    }
}
