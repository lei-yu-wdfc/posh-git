using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.FileStorage
{
    [TestFixture]
    [AUT(AUT.Uk)]
    public class SecciQueryTests
    {
        [Test]
        public void GetSecciQuery_Returns_SecciDocument()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            GetSecciQuery query = new GetSecciQuery()
                                      {
                                          ApplicationId = application.Id
                                      };
            var response = Drive.Api.Queries.Post(query);
        }
    }
    [Parallelizable(TestScope.All)]
    public class FileStorageServiceTests
    {
        [Test]
        public void FileStorageServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.FileStorage.IsRunning());
        }
    }
}
