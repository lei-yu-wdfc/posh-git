using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.CallValidate
{
    [Parallelizable(TestScope.All)]
    public class CallValidateServiceTests
    {
        [Test, AUT(AUT.Uk, AUT.Wb)]
        public void CallValidateServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.CallValidate.IsRunning());
        }
    }
}
