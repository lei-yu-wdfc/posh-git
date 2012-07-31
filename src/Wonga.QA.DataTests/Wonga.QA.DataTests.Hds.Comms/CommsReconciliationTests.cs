using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;

namespace Wonga.QA.DataTests.Hds.Comms
{
    [TestFixture(Order = 4)]
    [Category("Reconciliation")]
    [Category("Comms")]
    [Parallelizable]
    public class CommsReconciliationTests
    {
        [Test]
        [Description("Run the Comms reconcilliation and confirm that it succeeds")]
        public void RunReconciliationAndConfirmThatItSucceeds()
        {
            Reconciliation reconciliation = new Reconciliation();

            reconciliation.RunReconciliationAndConfirmThatItSucceeds(HdsUtilitiesBase.WongaService.Comms);
        }
    }
}
