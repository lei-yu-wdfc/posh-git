using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Email
{
    [Parallelizable(TestScope.All)]
    public class EmailServiceTests
    {
        [Test, AUT]
        public void EmailServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Email.IsRunning());
        }
    }
}
