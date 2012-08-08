using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Address.Queries.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;


namespace Wonga.QA.Tests.Address
{
    [Parallelizable(TestScope.All), Category("CoreTest"), AUT(AUT.Uk)]
    public class AddressTests
    {
        [Test, AUT(AUT.Uk), JIRA("UKWEB-1094"), Owner(Owner.CharlieBarker)]
        public void AddressLookupIsWorking()
        {
            var response = Drive.Api.Queries.Post(new GetAddressDescriptorsByPostCodeUkQuery()
                                       {CountryCode = "UK", Postcode = "NW1 7SN"});

            Assert.AreEqual(response.Values["Description"].First(), "1 Prince Albert Road, LONDON NW1 7SN", "These address descriptions should match");

            var descriptorId = response.Values["Id"].First();

            var secondResponse =
                Drive.Api.Queries.Post(new GetAddressByDescriptorIdUkQuery() {DescriptorId = descriptorId});

            Assert.AreEqual(secondResponse.Values["Postcode"].First(), "NW1 7SN", "Post code should match");
        }
    }
}
