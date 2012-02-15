using Gallio.Framework;
using MbUnit.Framework;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui
{
    public abstract class UiTest
    {
        public UiClient Client;

        [SetUp]
        public void SetUp()
        {
            Client = new UiClient();
        }

        [TearDown]
        public void TearDown()
        {
            //Note - I will take these out for now since I dont know HOW to see the attachement
            var name = TestContext.CurrentContext.Test.Name;
            //TestLog.EmbedImage(name + ".Screen", Client.Screen());
            TestLog.AttachHtml(name + ".Source", Client.Source());
            Client.Dispose();
        }
    }
}
