using MbUnit.Framework;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds
{
    [TestFixture]
    [DependsOn(typeof(SingleRowCrudTests))]
    class ReconciliationTests
    {
        [Test]
        [Category("Auto")]
        [Description("Tun the reconcilliation and confirm that it succeeds")]
        public void RunReconciliationAndConfirmThatItSucceeds()
        {
            // run initial load
            bool jobSuccess = SQLServerAgentJobs.Execute(HdsUtilities.HdsReconcileAgentJob);

            // check job succeeds
            Assert.IsTrue(jobSuccess);
        }

    }
}
