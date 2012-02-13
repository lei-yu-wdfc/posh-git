using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Experian
{
    [Parallelizable(TestScope.All)]
    public class ExperianServiceTests
    {
        [Test, AUT(AUT.Uk, AUT.Wb)]
        public void ExperianServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Experian.IsRunning());
        }
    }
}
