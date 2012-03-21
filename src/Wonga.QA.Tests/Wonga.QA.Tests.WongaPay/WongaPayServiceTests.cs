using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.WongaPay
{
    [Parallelizable(TestScope.All)]
    public class WongaPayServiceTests
    {
        [Test, AUT(AUT.Uk, AUT.Wb)]
        public void WongaPayServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.WongaPay.IsRunning());
        }
    }
}
