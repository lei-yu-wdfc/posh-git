using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;

namespace Wonga.QA.DataTests.Hds.Ops
{
    [TestFixture(Order = 4)]
    [Category("Reconciliation")]
    [Category("Ops")]
    [Parallelizable]
    public class OpsReconciliationTests
    {
        [Test]
        [Description("Run the Ops reconcilliation and confirm that it succeeds")]
        public void RunReconciliationAndConfirmThatItSucceeds()
        {
            Reconciliation reconciliation = new Reconciliation();

            reconciliation.RunReconciliationAndConfirmThatItSucceeds(HdsUtilities.WongaService.Ops);
        }
    }
}
