using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using System.Dynamic;

namespace Wonga.QA.Tests.Api
{
	[Parallelizable(TestScope.All)]
    public class ApiJourneys //We test this functionality everywhere!
	{
        [Test, AUT(AUT.Wb), Owner(Owner.AdrianMurphy)]
        public void WBL0JourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            Organisation comp = OrganisationBuilder.New(cust).Build();
            ApplicationBuilder.New(cust, comp).Build();
        }

        [Test, AUT(AUT.Wb), Owner(Owner.AdrianMurphy)]
        public void WBDeclinedL0Accepted()
        {
            Customer cust = CustomerBuilder.New().WithMiddleName("Middle").Build();

            Organisation comp = OrganisationBuilder.New(cust).Build();
            ApplicationBuilder.New(cust, comp).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za), Owner(Owner.AdrianMurphy)]
        public void L0JourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
        }

		[Test, AUT(AUT.Ca)]
		public void ApiNoMobilePhoneL0JourneyAccepted()
		{
			Customer cust = CustomerBuilder.New().WithMobileNumber(null).Build();
			ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(200).Build();
		}

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za), Owner(Owner.RiskTeam)]
        public void ApiL0JourneyDeclined()
        {
            Customer cust = CustomerBuilder.New().WithEmployer("Wonga").Build();
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

		[Test, AUT(AUT.Ca)]
		public void ApiNoMobilePhoneL0JourneyDeclined()
		{
			Customer cust = CustomerBuilder.New().WithEmployer("Wonga").WithMobileNumber(null).Build();
			ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za), Category(TestCategories.CoreTest), Owner(Owner.AdrianMurphy)]
        public void ApiLnJourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            var applicationL0 = ApplicationBuilder.New(cust).Build();

            applicationL0.RepayOnDueDate();

            ApplicationBuilder.New(cust).Build();
        }

		[Test, AUT(AUT.Ca)]
		public void ApiNoMobilePhoneLnJourneyAccepted()
		{
			Customer cust = CustomerBuilder.New().WithMobileNumber(null).Build();
			var applicationL0 = ApplicationBuilder.New(cust).Build();

			applicationL0.RepayOnDueDate();

			ApplicationBuilder.New(cust).Build();
		}

        [Test, AUT(AUT.Ca, AUT.Uk), Owner(Owner.AdrianMurphy)]
        public void ApiLnJourneyDeclined()
        {
            String forename = Get.RandomString(6);
            String surname = Get.RandomString(6);
            //L0
            var customer = CustomerBuilder.New().WithSurname(surname).WithForename(forename).Build();
            ApplicationBuilder.New(customer).Build().RepayOnDueDate();

            //LN - This is how we do it the PROPER WAY - watch and learn :) 
            var blackListTable = Drive.Data.Blacklist.Db.BlackList;
            dynamic blackListEntity = new ExpandoObject();
            blackListEntity.FirstName = forename;
            blackListEntity.LastName = surname;
            blackListTable.Insert(blackListEntity);

            CustomerOperations.UpdateEmployerNameInRisk(customer.Id,RiskMask.TESTBlacklist.ToString());
            ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

		[Test, AUT(AUT.Ca)]
		public void ApiNoMobilePhoneLnJourneyDeclined()
		{
			Customer cust = CustomerBuilder.New().WithMobileNumber(null).Build();
			ApplicationBuilder.New(cust).Build().RepayOnDueDate();
			CustomerOperations.UpdateEmployerNameInRisk(cust.Id, "Wonga");
			ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za), Owner(Owner.AdrianMurphy)]
		public void ApiRepayingOnDueDateClosesApplication()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			application.RepayOnDueDate();

			Assert.IsTrue(application.IsClosed);
		}
    }
}