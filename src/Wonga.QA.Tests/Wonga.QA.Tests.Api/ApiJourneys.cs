using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Api
{
    [Parallelizable(TestScope.All)]
    public class ApiJourneys
    {
        [Test, AUT(AUT.Wb)]
        public void WBApiL0JourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            Organisation comp = OrganisationBuilder.New(cust).Build();
            ApplicationBuilder.New(cust, comp).Build();
        }

        [Test, AUT(AUT.Wb)]
        public void WBApiDeclinedL0Accepted()
        {
            Customer cust = CustomerBuilder.New().WithMiddleName("Middle").Build();

            Organisation comp = OrganisationBuilder.New(cust).Build();
            ApplicationBuilder.New(cust, comp).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
        public void ApiL0JourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(200).Build();
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
        public void ApiL0JourneyDeclined()
        {
            Customer cust = CustomerBuilder.New().WithEmployer("Wonga").Build();
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
        public void ApiLnJourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            var applicationL0 = ApplicationBuilder.New(cust).Build();

            applicationL0.RepayOnDueDate();

            ApplicationBuilder.New(cust).Build();
        }

        [Test, AUT(AUT.Ca, AUT.Uk)]
        public void ApiLnJourneyDeclined()
        {
            Customer cust = CustomerBuilder.New().Build();
            ApplicationBuilder.New(cust).Build().RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(cust.Id,"Wonga");
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
		public void ApiRepayingOnDueDateClosesApplication()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			application.RepayOnDueDate();

			Assert.IsTrue(application.IsClosed);
		}
    }
}