using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds
{
    [TestFixture]
    class InitialLoadTests
    {
        [Test]
        public void RunInitialLoadAndConfirmThatItSucceeds()
        {
            // disable HDS load
            bool hdsAgentJobWasEnabled = HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob);

            // clear down HDS
            Drive.Data.Hds.Db.Payment.usp_ClearHdsTables();

            // run initial load
            bool jobSuccess = SQLServerAgentJobs.Execute(HdsUtilities.HdsInitialLoadAgentJob);

            // check job succeeds
            Assert.IsTrue(jobSuccess);

            // Reset HDS load
            if (hdsAgentJobWasEnabled)
            {
                HdsUtilities.EnableJob(HdsUtilities.HdsLoadAgentJob);
            }
        }
    }
}
