using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.ColdStorage
{
    [Parallelizable(TestScope.All)]
    public class ColdStorageServiceTests
    {
        [Test]
        public void ColdStorageServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.ColdStorage.IsRunning());
        }
    }
}
