using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.FileStorage
{
    [Parallelizable(TestScope.All)]
    public class FileStorageServiceTests
    {
        [Test]
        public void FileStorageServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.FileStorage.IsRunning());
        }
    }
}
