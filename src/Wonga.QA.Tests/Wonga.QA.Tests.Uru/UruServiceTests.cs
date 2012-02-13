using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Uru
{
    [Parallelizable(TestScope.All)]
    public class UruServiceTests
    {
        [Test, AUT(AUT.Uk, AUT.Wb)]
        public void UruServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Uru.IsRunning());
        }
    }
}
