using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds.Common
{
    public class InitialLoad
    {
        /// <summary>
        /// Run initial load and confirm that it succeeds
        /// </summary>
        /// <param name="wongaService">Service to run for</param>
        public void RunInitialLoadAndConfirmThatItSucceeds(HdsUtilitiesBase.WongaService wongaService)
        {
            HdsUtilitiesAgentJob hdsUtilitiesAgentJob = new HdsUtilitiesAgentJob(wongaService);

            // disable HDS load
            bool hdsAgentJobWasEnabled = hdsUtilitiesAgentJob.DisableJob(hdsUtilitiesAgentJob.HdsLoadAgentJob);
            bool hdsAgentJobWasExecuting = hdsUtilitiesAgentJob.StopJob(hdsUtilitiesAgentJob.HdsLoadAgentJob);

            // clear down HDS - not currently needed as initial load now includes cleardown
            //Drive.Data.Hds.Db.hds.usp_ClearHdsTables(hdsUtilities.WongaServiceSchema);

            // run initial load
            bool jobSuccess = SQLServerAgentJobs.Execute(hdsUtilitiesAgentJob.HdsInitialLoadAgentJob);

            // check job succeeds
            Assert.IsTrue(jobSuccess);

            // Reset HDS load
            if (hdsAgentJobWasEnabled)
            {
                hdsUtilitiesAgentJob.EnableJob(hdsUtilitiesAgentJob.HdsLoadAgentJob);
            }

            if (hdsAgentJobWasExecuting)
            {
                hdsUtilitiesAgentJob.StartJob(hdsUtilitiesAgentJob.HdsLoadAgentJob);
            }
        }
    }
}
