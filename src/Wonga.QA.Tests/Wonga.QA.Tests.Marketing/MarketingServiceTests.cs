using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Marketing
{
    [Parallelizable(TestScope.All)]
    public class MarketingServiceTests
    {
        [Test]
        public void MarketingServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.Marketing.IsRunning());
        }
    }
}
