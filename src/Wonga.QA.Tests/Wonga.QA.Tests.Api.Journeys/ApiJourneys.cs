﻿using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using MbUnit.Framework;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.ContactManagement;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Journeys
{
    //[Parallelizable(TestScope.All)]
    public class ApiJourneys
    {
        [Test, AUT(AUT.Wb)]
        public void WBApiL0JourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            
            Organisation comp = OrganisationBuilder.New().WithPrimaryApplicant(cust).Build();

            ApplicationBuilder.New(cust, comp).Build();
            SignupSecondaryDirectors(comp);
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
        public void ApiL0JourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            
            ApplicationBuilder.New(cust).Build();

        }

        [Test, AUT(AUT.Wb)]
        public void WBApiDeclinedL0Accepted()
        {
            Customer cust = CustomerBuilder.New().WithMiddleName("Middle").Build();

            Organisation comp = OrganisationBuilder.New().WithPrimaryApplicant(cust).Build();

            ApplicationBuilder.New(cust, comp).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
        public void ApiDeclinedL0Accepted()
        {
            Customer cust = CustomerBuilder.New().WithEmployer("Wonga").Build();

            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
        }

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
		public void ApiLnJourneyAccepted()
		{
			Customer cust = CustomerBuilder.New().Build();

			var applicationL0 = ApplicationBuilder.New(cust).Build();

			applicationL0.Repay();

			ApplicationBuilder.New(cust).Build();
		}

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
		public void ApiLnJourneyDeclined()
		{
			Customer cust = CustomerBuilder.New().Build();

			var applicationL0 = ApplicationBuilder.New(cust).Build();

			applicationL0.Repay();

			var db = new DbDriver();
			db.Risk.EmploymentDetails.Single(a => a.AccountId == cust.Id).EmployerName = "Wonga";
			db.Risk.SubmitChanges();

			ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();

		}
		


		#region Helpers

		private void SignupSecondaryDirectors(Organisation org)
        {
            var guarantors = Driver.Db.ContactManagement.DirectorOrganisationMappings.Where(entity => entity.OrganisationId == org.Id && entity.DirectorLevel>0);
            foreach (DirectorOrganisationMappingEntity guarantor in guarantors)
            {
                CustomerBuilder sd = CustomerBuilder.New(guarantor.AccountId);
                sd.Build();
            }
		}

		#endregion
	}
}