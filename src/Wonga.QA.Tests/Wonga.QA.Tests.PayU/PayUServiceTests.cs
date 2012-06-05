using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayU
{
    [Parallelizable(TestScope.All)]
    public class PayUServiceTests
    {
        [Test, AUT(AUT.Za), JIRA("ZA-2573", "ZA-2483")]
        public void PayUServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.PayU.IsRunning());
        }
    }
}
