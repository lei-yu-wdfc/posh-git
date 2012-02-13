using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.ContactManagement
{
    [Parallelizable(TestScope.All)]
    public class ContactManagementServiceTests
    {
        [Test, AUT(AUT.Wb)]
        public void ContactManagementServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.ContactManagement.IsRunning());
        }
    }
}
