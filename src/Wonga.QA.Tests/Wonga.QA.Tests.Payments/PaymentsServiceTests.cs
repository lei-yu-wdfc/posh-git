using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [Parallelizable(TestScope.All)]
    public class PaymentsServiceTests
    {
        [Test]
        public void PaymentsServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.Payments.IsRunning());
        }
    }
}
