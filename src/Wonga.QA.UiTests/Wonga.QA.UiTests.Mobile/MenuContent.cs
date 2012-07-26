using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Mobile
{
    public class MenuContent : UiMobileTest
    {
        [Test, AUT(AUT.Ca), JIRA("MBL-77"), Pending("Not deployed on Rc")]
        public void MenuTest()
        {
            var homepage = Client.MobileHome();
        }
    }
}
