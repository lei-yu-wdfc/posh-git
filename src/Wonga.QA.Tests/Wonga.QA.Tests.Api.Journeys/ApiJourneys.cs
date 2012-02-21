using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using MbUnit.Framework;
using Wonga.QA.Framework.Db.ContactManagement;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Journeys
{
    [Parallelizable(TestScope.All)]
    public class ApiJourneys
    {
        [Test, AUT(AUT.Wb)]
        public void WBApiL0Journey()
        {
            Customer cust = CustomerBuilder.New().Build();
            
            Organisation comp = OrganisationBuilder.New().WithPrimaryApplicant(cust).Build();

            ApplicationBuilder.New(cust, comp).Build();
            SignupSecondaryDirectors(comp);
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
        public void ApiL0Journey()
        {
            Customer cust = CustomerBuilder.New().Build();
            
            ApplicationBuilder.New(cust).Build();

        }

        [Test, AUT(AUT.Wb)]
        public void WBApiDeclinedL0()
        {
            Customer cust = CustomerBuilder.New().WithMiddleName("Middle").Build();

            Organisation comp = OrganisationBuilder.New().WithPrimaryApplicant(cust).Build();

            ApplicationBuilder.New(cust, comp).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
        public void ApiDeclinedL0()
        {
            Customer cust = CustomerBuilder.New().WithEmployer("Wonga").Build();

            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
        }

        private void SignupSecondaryDirectors(Organisation org)
        {
            var guarantors = Driver.Db.ContactManagement.DirectorOrganisationMappings.Where(entity => entity.OrganisationId == org.Id && entity.DirectorLevel>0);
            foreach (DirectorOrganisationMappingEntity guarantor in guarantors)
            {
                CustomerBuilder sd = CustomerBuilder.New(guarantor.AccountId);
                sd.Build();
            }
        }
    }
}