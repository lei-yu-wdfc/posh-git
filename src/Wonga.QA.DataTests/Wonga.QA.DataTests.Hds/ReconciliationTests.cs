using MbUnit.Framework;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds.Payments
{
    [TestFixture]
    [DependsOn(typeof(SingleRowCrudTests))]
    public class ReconciliationTests
    {
        [Test]
        [Category("Auto")]
        [Description("Run the reconcilliation and confirm that it succeeds")]
        public void RunReconciliationAndConfirmThatItSucceeds()
        {
            // run initial load
            bool jobSuccess = SQLServerAgentJobs.Execute(HdsUtilities.HdsReconcileAgentJob);

            // check job succeeds
            Assert.IsTrue(jobSuccess);
        }

    }
}
