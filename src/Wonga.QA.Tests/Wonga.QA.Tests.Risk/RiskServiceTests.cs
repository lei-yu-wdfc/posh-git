using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk
{
    [Parallelizable(TestScope.All)]
    public class RiskServiceTests
    {
        [Test]
        public void RiskServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Risk.IsRunning());
        }
    }
}
