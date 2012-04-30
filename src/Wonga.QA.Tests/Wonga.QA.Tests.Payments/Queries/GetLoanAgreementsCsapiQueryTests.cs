using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
//using CreateTransactionCommand = Wonga.QA.Framework.Cs.CreateTransactionCommand;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    public class GetLoanAgreementsCsapiQueryTests
    {
        //private dynamic _transactions = Drive.Data.Payments.Db.Transactions;
		private dynamic _applications = Drive.Data.Payments.Db.Applications;

        [Test]
        [AUT(AUT.Uk), JIRA("UK-1197")]
        public void Query_ShouldReturnNoLoans_WhenCustomersHasNoApplications()
        {
            Customer customer = CustomerBuilder.New().Build();
            var query = new GetLoanAgreementsQuery() {AccountId = customer.Id, IsActive = null};
            CsResponse response = Drive.Cs.Queries.Post(query);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-1197")]
        public void Query_ShouldReturnAllLoans_WhenCustomersHasApplications()
        {
            Customer customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            var query = new GetLoanAgreementsQuery() { AccountId = customer.Id, IsActive = null };
            CsResponse response = Drive.Cs.Queries.Post(query);
            Application[] applications = customer.GetApplications();
            //Assert.IsTrue(applications.Length, response.Values);

            foreach (var app in applications)
            {
                Assert.IsTrue(response.Values["ApplicationId"].SingleOrDefault(i=>i == app.Id.ToString()) != null);
            }
        }

		[Test, AUT(AUT.Za), JIRA("ZA-2360")]
		public void Close_InDuplumViolation_Application_BalanceShouldBeZero()
		{
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).
				Build();

			var loanApp = Do.Until(() => _applications.FindAllByExternalId(app.Id).Single());

			//fire refund transaction which cause InDuplum violation
			Drive.Msmq.Payments.Send(new Wonga.QA.Framework.Msmq.CreateTransactionCommand
			{
				Amount = 150,
				ApplicationId = app.Id,
				Currency = CurrencyCodeIso4217Enum.ZAR,
				ExternalId = Guid.NewGuid(),
				ComponentTransactionId = Guid.Empty,
				PostedOn = DateTime.UtcNow,
				Scope = PaymentTransactionScopeEnum.Debit,
				Source = PaymentTransactionSourceEnum.System,
				Type = PaymentTransactionEnum.Refund
			});

			//fire Direct Bank payment transaction to close off application so that compensating transaction 
			//will be created.
			Drive.Msmq.Payments.Send(new Wonga.QA.Framework.Msmq. CreateTransactionCommand
			{
				Amount = -200,
				ApplicationId = app.Id,
				Currency = CurrencyCodeIso4217Enum.ZAR,
				ExternalId = Guid.NewGuid(),
				ComponentTransactionId = Guid.Empty,
				PostedOn = DateTime.UtcNow,
				Scope = PaymentTransactionScopeEnum.Credit,
				Source = PaymentTransactionSourceEnum.System,
				Type = PaymentTransactionEnum.DirectBankPayment
			});

			//ClosedApplicationSaga timesout after 30sec before closing off application
			//Therefore delay is necessay
			Thread.Sleep(45000);


			var query = new GetLoanAgreementsQuery() { AccountId = customer.Id, IsActive = null };
			CsResponse response = Drive.Cs.Queries.Post(query);
			Application[] applications = customer.GetApplications();

			//Assert.IsNotNull(response.Values["ClosedOn"].SingleOrDefault());
			Assert.AreEqual<decimal>(0, decimal.Parse(response.Values["TodaysBalance"].SingleOrDefault()));
			Assert.AreEqual<decimal>(0, decimal.Parse(response.Values["FinalBalance"].SingleOrDefault()));
			
		}
    }
}