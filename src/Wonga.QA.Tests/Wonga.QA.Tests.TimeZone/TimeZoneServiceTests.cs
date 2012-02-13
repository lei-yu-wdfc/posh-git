using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.TimeZone
{
    [Parallelizable(TestScope.All)]
    public class TimeZoneServiceTests
    {
        [Test, AUT]
        public void TimeZoneServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.TimeZone.IsRunning());
        }
    }
}
