// ReSharper disable InconsistentNaming
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.FileStorage.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.FileStorage
{
    [TestFixture]
    [AUT(AUT.Uk), Parallelizable(TestScope.All)]
    public class SecciQueryTests
    {
        [Test]
        public void GetSecciQuery_Returns_SecciDocument()
        {
            // Arrange
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            var query = new GetSecciQuery
                            {
                                          ApplicationId = application.Id
                                      };

            // Act
            var response = Drive.Api.Queries.Post(query);

            // Assert
            Assert.IsTrue(response.Body.Length>0,"There should be something in the body");
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
