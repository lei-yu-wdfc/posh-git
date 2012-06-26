using MbUnit.Framework;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds
{
    [TestFixture]
    class ReconciliationTests
    {
        [Test]
        public void RunReconciliationAndConfirmThatItSucceeds()
        {
            // run initial load
            bool jobSuccess = SQLServerAgentJobs.Execute(HdsUtilities.HdsReconcileAgentJob);

            // check job succeeds
            Assert.IsTrue(jobSuccess);
        }

    }
}
