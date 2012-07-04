using MbUnit.Framework;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds.Payments
{
    [TestFixture(Order = 4)]
    public class ReconciliationTests
    {
        [Test]
        [Category("Reconcilliation")]
        [Description("Run the reconcilliation and confirm that it succeeds")]
        public void RunReconciliationAndConfirmThatItSucceeds()
        {
            SQLServerAgentJobs.WaitUntilJobComplete(HdsUtilities.CdcStagingAgentJob);
            SQLServerAgentJobs.WaitUntilJobComplete(HdsUtilities.HdsLoadAgentJob);

            bool hdsAgentJobWasEnabled = HdsUtilities.DisableJob(HdsUtilities.HdsReconcileAgentJob);

            bool jobSuccess = SQLServerAgentJobs.Execute(HdsUtilities.HdsReconcileAgentJob);
            
            // check job succeeds
            Assert.IsTrue(jobSuccess);

            if (hdsAgentJobWasEnabled)
            {
                HdsUtilities.EnableJob(HdsUtilities.HdsReconcileAgentJob);
            }

        }
    }
}
