using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.DataTests.Hds.BiCustomerManagement
{
    [TestFixture(Order = 4)]
    [Owner(Owner.JagjitThind)]
    [Category("HDSTest")]
    [Category("Reconciliation")]
    [Category("BiCustomerManagement")]
    [Parallelizable]
    public class BiCustomerManagementReconciliationTests
    {
        [Test]
        [Description("Run the BiCustomerManagement reconcilliation and confirm that it succeeds")]
        public void RunReconciliationAndConfirmThatItSucceeds()
        {
            Reconciliation reconciliation = new Reconciliation();

            reconciliation.RunReconciliationAndConfirmThatItSucceeds(HdsUtilitiesBase.WongaService.BiCustomerManagement);
        }
    }
}