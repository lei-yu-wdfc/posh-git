using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
	public class BusinessRemoveFromArrearsTests
	{
		[Test]
		public void BusinessApplicationShouldBeRemovedFromArrears_WhenTransactionReceivedPaysOffOutstandingArrearsAmount()
		{
			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutApplicationIntoArrears();

			var arrearsAmount = GetArrearsAmount(application.AccountId);

			//fire transaction for pay off arrears
			Drive.Msmq.Payments.Send(new CreateTransactionCommand
			{
				Amount = arrearsAmount,
				ApplicationId = application.Id,
				Currency = CurrencyCodeIso4217Enum.GBP,
				ExternalId = Guid.NewGuid(),
				ComponentTransactionId = Guid.Empty,
				PostedOn = DateTime.Now,
				Scope = PaymentTransactionScopeEnum.Credit,
				Source = PaymentTransactionSourceEnum.System,
				Type = PaymentTransactionEnum.Cheque
			});

			var applicationEntity = Do.Until(() => Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.SingleOrDefault(
					a => a.ApplicationEntity.ExternalId == application.Id));

			Do.Until(() => Drive.Db.Payments.Arrears.SingleOrDefault(a => a.ApplicationId == applicationEntity.ApplicationId) == null);
		}

		[Test]
		public void BusinessApplicationShouldNotBeRemovedFromArrears_WhenTransactionReceivedDoesNotPayOffEntireOutstandingArrearsAmount()
		{
			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutApplicationIntoArrears();

			var arrearsAmount = GetArrearsAmount(application.AccountId);

			//fire transaction for pay off arrears
			Drive.Msmq.Payments.Send(new CreateTransactionCommand
			{
				Amount = arrearsAmount - 1,
				ApplicationId = application.Id,
				Currency = CurrencyCodeIso4217Enum.GBP,
				ExternalId = Guid.NewGuid(),
				ComponentTransactionId = Guid.Empty,
				PostedOn = DateTime.Now,
				Scope = PaymentTransactionScopeEnum.Credit,
				Source = PaymentTransactionSourceEnum.System,
				Type = PaymentTransactionEnum.Cheque
			});

			Do.Until(() => Drive.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == application.Id
				&& t.Type == PaymentTransactionEnum.Cheque.ToString() && t.Amount == arrearsAmount -1));
	
			var applicationEntity = Do.Until(() => Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.SingleOrDefault(
					a => a.ApplicationEntity.ExternalId == application.Id));

			Assert.IsNotNull(Drive.Db.Payments.Arrears.SingleOrDefault(a => a.ApplicationId == applicationEntity.ApplicationId));
		}

		[Test]
		public void BusinessApplicationShouldNotBeRemovedFromArrears_WhenTransactionReceivedIsNotExpectedType()
		{
			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutApplicationIntoArrears();

			var arrearsAmount = GetArrearsAmount(application.AccountId);

			//fire transaction for pay off arrears
			Drive.Msmq.Payments.Send(new CreateTransactionCommand
			{
				Amount = arrearsAmount,
				ApplicationId = application.Id,
				Currency = CurrencyCodeIso4217Enum.GBP,
				ExternalId = Guid.NewGuid(),
				ComponentTransactionId = Guid.Empty,
				PostedOn = DateTime.Now,
				Scope = PaymentTransactionScopeEnum.Credit,
				Source = PaymentTransactionSourceEnum.System,
				Type = PaymentTransactionEnum.Fee
			});

			Do.Until(() => Drive.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == application.Id
				&& t.Type == PaymentTransactionEnum.Fee.ToString() && t.Amount == arrearsAmount));

			var applicationEntity = Do.Until(() => Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.SingleOrDefault(
					a => a.ApplicationEntity.ExternalId == application.Id));

			Assert.IsNotNull(Drive.Db.Payments.Arrears.SingleOrDefault(a => a.ApplicationId == applicationEntity.ApplicationId));	
		}

		[Test]
		public void BusinessApplicationShouldNotBeRemovedFromArrears_WhenTransactionReceivedIsNotExpectedScope()
		{
			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutApplicationIntoArrears();

			var arrearsAmount = GetArrearsAmount(application.AccountId);

			//fire transaction for pay off arrears
			Drive.Msmq.Payments.Send(new CreateTransactionCommand
			{
				Amount = arrearsAmount,
				ApplicationId = application.Id,
				Currency = CurrencyCodeIso4217Enum.GBP,
				ExternalId = Guid.NewGuid(),
				ComponentTransactionId = Guid.Empty,
				PostedOn = DateTime.Now,
				Scope = PaymentTransactionScopeEnum.Debit,
				Source = PaymentTransactionSourceEnum.System,
				Type = PaymentTransactionEnum.Cheque
			});

			Do.Until(() => Drive.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == application.Id
				&& t.Scope == (int)PaymentTransactionScopeEnum.Debit && t.Amount == arrearsAmount));

			var applicationEntity = Do.Until(() => Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.SingleOrDefault(
					a => a.ApplicationEntity.ExternalId == application.Id));

			Assert.IsNotNull(Drive.Db.Payments.Arrears.SingleOrDefault(a => a.ApplicationId == applicationEntity.ApplicationId));
		}

		private static decimal GetArrearsAmount(Guid accountId)
		{
			var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
			{
				AccountId = accountId
			});
			return decimal.Parse(response.Values["Arrears"].Single());
		}
	}
}
