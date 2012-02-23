using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ops
{
    [Parallelizable(TestScope.All)]
    public class OpsServiceTests
    {
        [Test]
        public void OpsServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Ops.IsRunning());
        }
    }
}
