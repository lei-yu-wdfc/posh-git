﻿using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;

namespace Wonga.QA.DataTests.Hds.Payments
{
    [TestFixture(Order = 1)]
    [Category("HDSTest")]
    [Category("InitialLoad")]
    [Category("Payments")]
    [Parallelizable(TestScope.All)]
    public class PaymentsInitialLoadTests
    {
        [Test]
        [Description("Run Payments initial load and confirm that it succeeds")]
        public void RunInitialLoadAndConfirmThatItSucceeds()
        {
            InitialLoad initialLoad = new InitialLoad();

            initialLoad.RunInitialLoadAndConfirmThatItSucceeds(HdsUtilitiesBase.WongaService.Payments);
        }
    }
}
