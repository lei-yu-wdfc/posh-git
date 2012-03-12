using MbUnit.Framework;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Meta
{
    [Parallelizable(TestScope.All)]
    public class BuilderTests
    {
        private Customer _customer;

        [Test]
        public void CustomerBuilderTest()
        {
            Assert.DoesNotThrow(() => _customer = CustomerBuilder.New().Build());
        }

        [Test, DependsOn("CustomerBuilderTest")]
        public void ApplicationBuilderTest()
        {
            Assert.DoesNotThrow(() => ApplicationBuilder.New(_customer).Build());
        }
    }
}
