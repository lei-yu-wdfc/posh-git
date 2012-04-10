using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.ContactManagement;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture]
    [Parallelizable(TestScope.All), AUT(AUT.Wb)]
    public class OrganisationTests
    {
        [Test, AUT(AUT.Wb)]
        public void TestOrganisationIsBuilt_DirectorsPersisted()
        {
            var primaryApplicant = CustomerBuilder.New().Build();
            var listOfGuarantors = new List<CustomerBuilder>()
                                       {
                                           CustomerBuilder.New(),
                                           CustomerBuilder.New(),
                                           CustomerBuilder.New()
                                       };

            var org = OrganisationBuilder.New(primaryApplicant).Build();
            var application = (ApplicationBuilder.New(primaryApplicant, org) as BusinessApplicationBuilder).WithGuarantors(listOfGuarantors).Build();
            IEnumerable<DirectorOrganisationMappingEntity> list = org.GetSecondaryDirectors();
            
            Assert.IsTrue(list.Count()==3);
        }
    }
}
