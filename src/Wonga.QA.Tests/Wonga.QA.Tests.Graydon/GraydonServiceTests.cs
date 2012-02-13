using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Graydon
{
    [Parallelizable(TestScope.All)]
    public class GraydonServiceTests
    {
        [Test, AUT(AUT.Wb)]
        public void GraydonServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Graydon.IsRunning());
        }
    }
}
