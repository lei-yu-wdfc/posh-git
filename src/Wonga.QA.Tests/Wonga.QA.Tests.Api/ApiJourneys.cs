using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Api
{
	[Parallelizable(TestScope.All), Pending("ZA-2565")]
    public class ApiJourneys //We test this functionality everywhere!
	{
        [Test, AUT(AUT.Wb)]
        public void WBL0JourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            Organisation comp = OrganisationBuilder.New(cust).Build();
            ApplicationBuilder.New(cust, comp).Build();
        }

        [Test, AUT(AUT.Wb)]
        public void WBDeclinedL0Accepted()
        {
            Customer cust = CustomerBuilder.New().WithMiddleName("Middle").Build();

            Organisation comp = OrganisationBuilder.New(cust).Build();
            ApplicationBuilder.New(cust, comp).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

		[Test, AUT(AUT.Ca, AUT.Uk)]
        public void L0JourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
        }

		[Test, AUT(AUT.Ca, AUT.Uk), Pending("ZA-2565")]
        public void L0JourneyDeclined()
        {
            Customer cust = CustomerBuilder.New().WithEmployer("Wonga").Build();
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

		[Test, AUT(AUT.Ca, AUT.Uk), Pending("ZA-2565")]
        public void LnJourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            var applicationL0 = ApplicationBuilder.New(cust).Build();

            applicationL0.RepayOnDueDate();

            ApplicationBuilder.New(cust).Build();
        }

		[Test, AUT(AUT.Ca, AUT.Uk), Pending("ZA-2565")]
        public void LnJourneyDeclined()
        {
            Customer cust = CustomerBuilder.New().Build();
            ApplicationBuilder.New(cust).Build().RepayOnDueDate();

            Drive.Db.UpdateEmployerName(cust.Id, "Wonga");
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

		[Test, AUT(AUT.Ca, AUT.Uk), Pending("ZA-2565")]
		public void RepayingOnDueDateClosesApplication()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			application.RepayOnDueDate();

			Assert.IsTrue(application.IsClosed);
		}
    }
}