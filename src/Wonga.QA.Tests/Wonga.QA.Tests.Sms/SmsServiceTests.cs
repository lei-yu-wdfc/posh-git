using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Sms
{
    [Parallelizable(TestScope.All)]
    public class SmsServiceTests
    {
        [Test]
        public void SmsServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.Sms.IsRunning());
        }
    }
}
