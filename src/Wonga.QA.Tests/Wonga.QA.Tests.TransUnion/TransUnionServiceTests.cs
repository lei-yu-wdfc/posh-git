using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.TransUnion
{
    [Parallelizable(TestScope.All)]
    public class TransUnionServiceTests
    {
        [Test, AUT(AUT.Za)]
        public void TransUnionServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.TransUnion.IsRunning());
        }
    }
}
