using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Meta
{
    [Parallelizable(TestScope.All)]
    public class BuilderTests
    {
        private Customer _customer;
        private Organisation _organisation;

        [Test]
        public void CustomerBuilderTest()
        {
            Assert.DoesNotThrow(() => _customer = CustomerBuilder.New().Build());
        }

        [Test, AUT(AUT.Wb), DependsOn("CustomerBuilderTest")]
        public void OrganisationBuilderTest()
        {
            Assert.DoesNotThrow(() => _organisation = OrganisationBuilder.New(_customer).Build());
        }

        [Test, DependsOn("CustomerBuilderTest"), DependsOn("OrganisationBuilderTest")]
        public void ApplicationBuilderTest()
        {
            ApplicationBuilder builder = Config.AUT == AUT.Wb ?
                ApplicationBuilder.New(_customer, _organisation) :
                ApplicationBuilder.New(_customer);
            Assert.DoesNotThrow(() => builder.Build());
        }
    }
}
