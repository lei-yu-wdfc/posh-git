using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [Parallelizable(TestScope.All)]
    public class CommsServiceTests
    {
        [Test, AUT]
        public void CommsServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Comms.IsRunning());
        }
    }
}
