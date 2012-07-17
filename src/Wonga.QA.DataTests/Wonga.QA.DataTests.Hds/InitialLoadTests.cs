using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds.Payments
{
    [TestFixture(Order = 1)]
    public class InitialLoadTests
    {
        [Test]
        [Category("InitialLoad")]
        [Description("Run inital load and confirm that it succeeds")]
        public void RunInitialLoadAndConfirmThatItSucceeds()
        {
            // disable HDS load
            bool hdsAgentJobWasEnabled = HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob);

            // clear down HDS
            Drive.Data.Hds.Db.hds.usp_ClearHdsTables("payment");

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
