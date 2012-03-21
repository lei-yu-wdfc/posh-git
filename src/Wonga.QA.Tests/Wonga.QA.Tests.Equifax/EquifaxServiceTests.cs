using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Equifax
{
    [Parallelizable(TestScope.All)]
    public class EquifaxServiceTests
    {
        [Test, AUT(AUT.Ca)]
        public void EquifaxServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.Equifax.IsRunning());
        }
    }
}
