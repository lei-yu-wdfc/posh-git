using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Hpi
{
    [Parallelizable(TestScope.All)]
    public class HpiServiceTests
    {
        [Test, AUT(AUT.Uk, AUT.Wb)]
        public void HpiServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Hpi.IsRunning());
        }
    }
}
