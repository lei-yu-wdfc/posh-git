using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;

namespace Wonga.QA.DataTests.Hds.Ops
{
    [TestFixture(Order = 1)]
    [Category("InitialLoad")]
    [Category("Ops")]
    [Parallelizable(TestScope.All)]
    public class OpsInitialLoadTests
    {
        [Test]
        [Description("Run Ops initial load and confirm that it succeeds")]
        public void RunInitialLoadAndConfirmThatItSucceeds()
        {
            InitialLoad initialLoad = new InitialLoad();

            initialLoad.RunInitialLoadAndConfirmThatItSucceeds(HdsUtilitiesBase.WongaService.Ops);
        }
    }
}
