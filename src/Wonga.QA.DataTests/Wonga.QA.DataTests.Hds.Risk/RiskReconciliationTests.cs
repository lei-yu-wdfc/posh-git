using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;

namespace Wonga.QA.DataTests.Hds.Risk
{
    [TestFixture(Order = 4)]
    [Category("Reconciliation")]
    [Category("Risk")]
    [Parallelizable(TestScope.All)]
    public class RiskReconciliationTests
    {
        [Test]
        [Description("Run the Risk reconcilliation and confirm that it succeeds")]
        public void RunReconciliationAndConfirmThatItSucceeds()
        {
            Reconciliation reconciliation = new Reconciliation();

            reconciliation.RunReconciliationAndConfirmThatItSucceeds(HdsUtilities.WongaService.Risk);
        }
    }
}
