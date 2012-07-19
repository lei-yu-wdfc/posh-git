using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;

namespace Wonga.QA.DataTests.Hds.Comms
{
    [TestFixture(Order = 1)]
    [Category("InitialLoad")]
    [Category("Comms")]
    [Parallelizable(TestScope.All)]
    public class CommsInitialLoadTests
    {
        [Test]
        [Description("Run Comms initial load and confirm that it succeeds")]
        public void RunInitialLoadAndConfirmThatItSucceeds()
        {
            InitialLoad initialLoad = new InitialLoad();

            initialLoad.RunInitialLoadAndConfirmThatItSucceeds(HdsUtilities.WongaService.Comms);
        }
    }
}
