using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.FileStorage.Queries.Wb.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture, AUT(AUT.Wb), Parallelizable(TestScope.All)]
    public class GuarantorAgreementTests
    {
        [AUT(AUT.Wb)]
        [Test]
        [Ignore("Should be retested")]
        public void GetGuarantorAgreementQuery_Returns_NonEmptyAgreementContent()
        {
            var customer = CustomerBuilder.New().Build();
            var guarantorBuilder = CustomerBuilder.New();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        guarantorBuilder,
                                    };
            var organisation = OrganisationBuilder.New(customer).Build();
            var applicationInfo = ((BusinessApplicationBuilder)ApplicationBuilder
                .New(customer, organisation))
                .WithGuarantors(guarantorList).Build() as BusinessApplication;

            var query = new GetGuarantorAgreementWbUkQuery() 
            {
                ApplicationId = applicationInfo.Id,
                GuarantorAccountId = guarantorBuilder.Id
            };

            var response = Drive.Api.Queries.Post(query);
        }
    }
}