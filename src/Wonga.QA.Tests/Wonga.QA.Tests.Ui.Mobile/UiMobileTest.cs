using Gallio.Framework;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Mobile
{
    public abstract class UiMobileTest
    {
        public TestLocal<MobileUiClient> _Client = new TestLocal<MobileUiClient>(() => new MobileUiClient());

        public MobileUiClient Client
        {
            get { return _Client.Value; }
        }
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                var name = TestContext.CurrentContext.Test.Name;

                if (!Config.Ui.RemoteMode)
                    TestLog.EmbedImage(name + ".Screen", Client.Screen());
                TestLog.AttachHtml(name + ".Source", Client.Source());
            }
            finally
            {
                Client.Dispose();
            }
        }
    }
}
