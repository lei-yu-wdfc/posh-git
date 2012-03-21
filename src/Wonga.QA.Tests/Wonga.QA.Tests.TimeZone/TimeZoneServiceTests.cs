using MbUnit.Framework;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.TimeZone
{
    [Parallelizable(TestScope.All)]
    public class TimeZoneServiceTests
    {
        [Test]
        public void TimeZoneServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.TimeZone.IsRunning());
        }
    }
}
