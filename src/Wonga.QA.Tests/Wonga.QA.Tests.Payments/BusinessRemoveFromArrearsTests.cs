using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Tests.Core;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionScopeEnum;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
    [Parallelizable(TestScope.All)]
	public class BusinessRemoveFromArrearsTests
	{
		[Test, AUT(AUT.Wb), JIRA("SME-892")]
		public void BusinessApplicationShouldBeRemovedFromArrears_WhenTransactionReceivedPaysOffOutstandingArrearsAmount()
		{
			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutIntoArrears() as BusinessApplication;

			var arrearsAmount = application.GetArrearsAmount();

			//fire transaction for pay off arrears
			Drive.Msmq.Payments.Send(new CreateTransaction
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

		[Test, AUT(AUT.Wb), JIRA("SME-892")]
		public void BusinessApplicationShouldNotBeRemovedFromArrears_WhenTransactionReceivedDoesNotPayOffEntireOutstandingArrearsAmount()
		{
			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutIntoArrears() as BusinessApplication;

			var arrearsAmount = application.GetArrearsAmount();

			//fire transaction for pay off arrears
			Drive.Msmq.Payments.Send(new CreateTransaction
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

			Thread.Sleep(15000);

			var applicationEntity = Do.Until(() => Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.SingleOrDefault(
					a => a.ApplicationEntity.ExternalId == application.Id));

			Assert.IsNotNull(Drive.Db.Payments.Arrears.SingleOrDefault(a => a.ApplicationId == applicationEntity.ApplicationId));
		}

		[Test, AUT(AUT.Wb), JIRA("SME-892")]
		public void BusinessApplicationShouldNotBeRemovedFromArrears_WhenTransactionReceivedIsNotExpectedType()
		{
			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutIntoArrears() as BusinessApplication;

			var arrearsAmount = application.GetArrearsAmount();

			//fire transaction for pay off arrears
			Drive.Msmq.Payments.Send(new CreateTransaction
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

			Thread.Sleep(15000);

			var applicationEntity = Do.Until(() => Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.SingleOrDefault(
					a => a.ApplicationEntity.ExternalId == application.Id));

			Assert.IsNotNull(Drive.Db.Payments.Arrears.SingleOrDefault(a => a.ApplicationId == applicationEntity.ApplicationId));	
		}

		[Test, AUT(AUT.Wb), JIRA("SME-892")]
		public void BusinessApplicationShouldNotBeRemovedFromArrears_WhenTransactionReceivedIsNotExpectedScope()
		{
			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutIntoArrears() as BusinessApplication;

			var arrearsAmount = application.GetArrearsAmount();

			//fire transaction for pay off arrears
			Drive.Msmq.Payments.Send(new CreateTransaction
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

			Thread.Sleep(15000);

			var applicationEntity = Do.Until(() => Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.SingleOrDefault(
					a => a.ApplicationEntity.ExternalId == application.Id));

			Assert.IsNotNull(Drive.Db.Payments.Arrears.SingleOrDefault(a => a.ApplicationId == applicationEntity.ApplicationId));
		}
	}
}
