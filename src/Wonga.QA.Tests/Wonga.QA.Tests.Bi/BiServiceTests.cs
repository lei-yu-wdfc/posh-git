using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Bi
{
    [Parallelizable(TestScope.All)]
    public class BiServiceTests
    {
        [Test]
        public void BiServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Bi.IsRunning());
        }
    }
}
