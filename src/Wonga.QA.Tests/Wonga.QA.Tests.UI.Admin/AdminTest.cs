using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui.Admin
{
    public abstract class AdminTest
    {
        public UiClient Client;

        [SetUp]
        public void SetUp()
        {
            
        }

        [TearDown]
        public void TearDown()
        {
            //var name = TestContext.CurrentContext.Test.Name;
            //TestLog.EmbedImage(name + ".Screen", Client.Screen());
            //TestLog.AttachHtml(name + ".Source", Client.Source());
            //Client.Dispose();
        }
    }
}
