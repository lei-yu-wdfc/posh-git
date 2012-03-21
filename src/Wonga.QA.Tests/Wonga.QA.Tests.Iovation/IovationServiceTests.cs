using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Iovation
{
    [Parallelizable(TestScope.All)]
    public class IovationServiceTests
    {
        [Test]
        public void IovationServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.Iovation.IsRunning());
        }
    }
}
