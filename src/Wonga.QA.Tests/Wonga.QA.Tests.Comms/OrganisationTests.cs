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
        [Ignore]
        public void TestOrganisationIsBuilt_DirectorsPersisted()
        {
            Customer primaryApplicant = CustomerBuilder.New().Build();
            OrganisationBuilder organisationBuilder =
                OrganisationBuilder.New(primaryApplicant).WithSoManySecondaryDirectors(2);

            Organisation org = organisationBuilder.Build();

            Thread.Sleep(10000);
            IEnumerable<DirectorOrganisationMappingEntity> list = org.GetSecondaryDirectors();
            
            Assert.IsTrue(list.Count()==2);
        }
    }
}
