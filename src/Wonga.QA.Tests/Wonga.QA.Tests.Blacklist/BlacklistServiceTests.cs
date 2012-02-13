using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Blacklist
{
    [Parallelizable(TestScope.All)]
    public class BlacklistServiceTests
    {
        [Test, AUT]
        public void BlacklistServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Blacklist.IsRunning());
        }
    }
}
