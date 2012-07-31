using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;

namespace Wonga.QA.DataTests.Hds.Risk
{
    [TestFixture(Order = 1)]
    [Category("InitialLoad")]
    [Category("Risk")]
    [Parallelizable(TestScope.All)]
    public class RiskInitialLoadTests
    {
        [Test]
        [Description("Run Risk initial load and confirm that it succeeds")]
        public void RunInitialLoadAndConfirmThatItSucceeds()
        {
            InitialLoad initialLoad = new InitialLoad();

            initialLoad.RunInitialLoadAndConfirmThatItSucceeds(HdsUtilitiesBase.WongaService.Risk);
        }
    }
}
