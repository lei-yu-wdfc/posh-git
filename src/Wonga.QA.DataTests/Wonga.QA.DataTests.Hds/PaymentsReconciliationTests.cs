﻿using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;

namespace Wonga.QA.DataTests.Hds.Payments
{
    [TestFixture(Order = 4)]
    [Category("HDSTest")]
    [Category("Reconciliation")]
    [Category("Payments")]
    [Parallelizable]
    public class PaymentsReconciliationTests
    {
        [Test]
        [Description("Run the Payments reconcilliation and confirm that it succeeds")]
        public void RunReconciliationAndConfirmThatItSucceeds()
        {
            Reconciliation reconciliation = new Reconciliation();

            reconciliation.RunReconciliationAndConfirmThatItSucceeds(HdsUtilitiesBase.WongaService.Payments);
        }
    }
}
