using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Meta
{
    [TestFixture, Parallelizable(TestScope.All), DependsOn(typeof(ColdStartTests)), Category(TestCategories.CoreTest)]
    public class BuilderTests
    {
        private Customer _customer;
        private Organisation _organisation;

        [Test, Owner(Owner.StanDesyatnikov)]
        public void CustomerBuilderTest()
        {
            Assert.DoesNotThrow(() => _customer = CustomerBuilder.New().Build());
        }

        [Test, AUT(AUT.Ca, AUT.Pl, AUT.Uk, AUT.Za), DependsOn("CustomerBuilderTest"), Owner(Owner.StanDesyatnikov)]
        public void ApplicationBuilderTest()
        {
            ApplicationBuilder builder = ApplicationBuilder.New(_customer);

            builder.Build();
        }
        [Test, AUT(AUT.Wb), DependsOn("OrganisationBuilderTest")]
        public void WbApplicationBuilderTest()
        {
            ApplicationBuilder builder = ApplicationBuilder.New(_customer, _organisation);

            builder.Build();
        }

		[Test, AUT(AUT.Wb), DependsOn("CustomerBuilderTest"), Owner(Owner.AdrianMurphy)]
		public void OrganisationBuilderTest()
		{
			Assert.DoesNotThrow(() => _organisation = OrganisationBuilder.New(_customer).Build());
		}
    }
}
