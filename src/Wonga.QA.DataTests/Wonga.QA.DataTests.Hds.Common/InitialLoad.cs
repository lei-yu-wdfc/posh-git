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
        public void RunInitialLoadAndConfirmThatItSucceeds(HdsUtilities.WongaService wongaService)
        {
            HdsUtilities hdsUtilities = new HdsUtilities(wongaService);

            // disable HDS load
            bool hdsAgentJobWasEnabled = hdsUtilities.DisableJob(hdsUtilities.HdsLoadAgentJob);

            // clear down HDS - not currently needed as initial load now includes cleardown
            //Drive.Data.Hds.Db.hds.usp_ClearHdsTables(hdsUtilities.WongaServiceSchema);

            // run initial load
            bool jobSuccess = SQLServerAgentJobs.Execute(hdsUtilities.HdsInitialLoadAgentJob);

            // check job succeeds
            Assert.IsTrue(jobSuccess);

            // Reset HDS load
            if (hdsAgentJobWasEnabled)
            {
                hdsUtilities.EnableJob(hdsUtilities.HdsLoadAgentJob);
            }
        }
    }
}
