﻿using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Core;
using AddBankAccountUkCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk.AddBankAccountUkCommand;
using AddPaymentCardCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.AddPaymentCardCommand;
using CreateBusinessFixedInstallmentLoanApplicationWbUkCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.Wb.Uk.CreateBusinessFixedInstallmentLoanApplicationWbUkCommand;
using GenderEnum = Wonga.QA.Framework.Api.Enums.GenderEnum;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionScopeEnum;
using SignBusinessApplicationWbUkCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.Wb.Uk.SignBusinessApplicationWbUkCommand;

namespace Wonga.QA.Tests.Payments
{
	[Parallelizable(TestScope.All)]
	public class BusinessAccountSummaryTests
	{
		/// <summary>
		/// Given that the customer is on the accounts summary page
		/// When the loan application has not been signed by all guarantors
		/// Then the loan should be displayed as in progress.
		/// </summary>

        private const string RiskBasedPricingEnabled = "Payments.Wb.RiskBasedPricingEnabled";
        private const bool RiskBasedPricingStatusEnabled = true;
		[Test, AUT(AUT.Wb), JIRA("SME-251")]
		public void GetBusinessAccountSummary_ShouldNotLoadLoanSummaryDetails_WhenApplicationIsNotSignedByAllGuarantors()
		{
			var applicationId = Guid.NewGuid();
			var accountId = Guid.NewGuid();
			var organisationId = Guid.NewGuid();
			var paymentCardId = Guid.NewGuid();
			var bankAccountId = Guid.NewGuid();

			Drive.Api.Commands.Post(new ApiRequest[]
			                         	{
											AddBankAccountUkCommand.New(a => { a.AccountId = accountId;
											                                 	a.BankAccountId = bankAccountId;
											}),
											AddPaymentCardCommand.New(a => { a.AccountId = accountId;
											                               	a.PaymentCardId = paymentCardId;
											}),
											CreateBusinessFixedInstallmentLoanApplicationWbUkCommand.New(a =>
											{
												a.AccountId = accountId;
												a.ApplicationId = applicationId;
												a.BusinessBankAccountId = bankAccountId;
												a.BusinessPaymentCardId = paymentCardId;
												a.OrganisationId = organisationId;																
											})
			                         	});

			Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == applicationId));

			var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
													{
														AccountId = accountId
													});
			Assert.IsNotNull(response);
			Assert.IsNotNull(response.Values["ApplicationId"]);
			Assert.AreEqual("false", response.Values["IsApplicationSigned"].SingleOrDefault());
			Assert.AreEqual("false", response.Values["IsApplicationAccepted"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["Arrears"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["PrincipalLoanAmount"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["LoanTerm"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["WeeklyRepaymentAmount"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["NumberOfPaymentsMade"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["RemainingNumberOfPayments"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["TotalOutstandingAmount"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["OutstandingPrincipalAmount"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["OutstandingFees"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["OutstandingInterest"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["Arrears"].SingleOrDefault());
		}

		/// <summary>
		/// Given that the customer is on the accounts summary page
		/// When the loan application has not been approved
		///Then the loan should be displayed as in progress.
		/// </summary>
		[Test, AUT(AUT.Wb), JIRA("SME-251")]
		public void GetBusinessAccountSummary_ShouldNotLoadLoanSummaryDetails_WhenApplicationIsNotAccepted()
		{
			var applicationId = Guid.NewGuid();
			var accountId = Guid.NewGuid();
			var organisationId = Guid.NewGuid();
			var paymentCardId = Guid.NewGuid();
			var bankAccountId = Guid.NewGuid();

			Drive.Api.Commands.Post(new ApiRequest[]
			                            {
			                                AddBankAccountUkCommand.New(a => { a.AccountId = accountId;
			                                                                a.BankAccountId = bankAccountId;
			                                }),
			                                AddPaymentCardCommand.New(a => { a.AccountId = accountId;
			                                                                a.PaymentCardId = paymentCardId;
			                                }),
			                                CreateBusinessFixedInstallmentLoanApplicationWbUkCommand.New(a =>
			                                {
			                                    a.AccountId = accountId;
			                                    a.ApplicationId = applicationId;
			                                    a.BusinessBankAccountId = bankAccountId;
			                                    a.BusinessPaymentCardId = paymentCardId;
			                                    a.OrganisationId = organisationId;																
			                                })
			                            });

			Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == applicationId));

			Drive.Api.Commands.Post(new ApiRequest[]
			                            {
			                                    new SignBusinessApplicationWbUkCommand
			                                    {
			                                        ApplicationId = applicationId,
			                                        AccountId = accountId
			                                    } 
			                            });

			Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == applicationId && a.SignedOn != null));

			var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
			{
				AccountId = accountId
			});

			Assert.IsNotNull(response);
			Assert.IsNotNull(response.Values["ApplicationId"]);
			Assert.AreEqual("true", response.Values["IsApplicationSigned"].SingleOrDefault());
			Assert.AreEqual("false", response.Values["IsApplicationAccepted"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["Arrears"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["PrincipalLoanAmount"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["LoanTerm"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["WeeklyRepaymentAmount"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["NumberOfPaymentsMade"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["RemainingNumberOfPayments"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["TotalOutstandingAmount"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["OutstandingPrincipalAmount"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["OutstandingFees"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["OutstandingInterest"].SingleOrDefault());
			Assert.AreEqual("0", response.Values["Arrears"].SingleOrDefault());
		}

		/// <summary>
		/// Given that the customer is on the accounts summary page
		/// When the loan is approved
		/// And When the loan terms have been signed by all guarantors
		/// Then the arrears amount and no other details should be displayed to the customer
		/// </summary>
		[Test, AUT(AUT.Wb), JIRA("SME-251")]
		public void GetBusinessAccountSummary_ShouldReturnAccountDetails_WhenLoanIsInProgress()
		{
            var riskBasedPricingStatus = Drive.Data.Ops.GetServiceConfiguration<bool>(RiskBasedPricingEnabled);
            Drive.Data.Ops.SetServiceConfiguration<bool>(RiskBasedPricingEnabled, RiskBasedPricingStatusEnabled);

		    var customer =
		        CustomerBuilder.New().WithNumberOfDependants(0).WithGender(GenderEnum.Female).WithSpecificAge(40).Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanTerm(20).Build();

			Do.Until(() =>
				GetTransactionCount(
				t => application.Id == t.ApplicationEntity.ExternalId && t.Scope == (int)PaymentTransactionScopeEnum.Debit
						&& t.Type == PaymentTransactionEnum.Fee.ToString()));

			Do.With.Timeout(TimeSpan.FromSeconds(5)).Until(() =>
				GetTransactionCount(
					t => application.Id == t.ApplicationEntity.ExternalId && t.Scope == (int)PaymentTransactionScopeEnum.Debit
						 && t.Type == PaymentTransactionEnum.Interest.ToString()));

			Do.With.Timeout(TimeSpan.FromSeconds(5)).Until(() =>
				GetTransactionCount(
					t => application.Id == t.ApplicationEntity.ExternalId && t.Scope == (int)PaymentTransactionScopeEnum.Debit
						 && t.Type == PaymentTransactionEnum.CashAdvance.ToString()));

			Do.Until(() => Drive.Db.Payments.PaymentPlans.Single(pp => pp.ApplicationEntity.ExternalId == application.Id));

			var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
			{
				AccountId = customer.Id
			});

			Assert.IsNotNull(response);
			Assert.IsNotNull(response.Values["ApplicationId"]);
			Assert.AreEqual("true", response.Values["IsApplicationSigned"].SingleOrDefault(), "Expected IsApplicationSigned value is incorrect.");
			Assert.AreEqual("true", response.Values["IsApplicationAccepted"].SingleOrDefault(), "Expected IsApplicationAccepted value is incorrect.");
			Assert.AreEqual("0", response.Values["Arrears"].SingleOrDefault(), "Expected Arrears value is incorrect.");
			Assert.AreEqual("10000.00", response.Values["PrincipalLoanAmount"].SingleOrDefault(), "Expected PrincipalLoanAmount value is incorrect.");
			Assert.AreEqual("20", response.Values["LoanTerm"].SingleOrDefault(), "Expected LoanTerm value is incorrect.");
			Assert.AreEqual("725.00", response.Values["WeeklyRepaymentAmount"].SingleOrDefault(), "Expected WeeklyRepaymentAmount value is incorrect.");
			Assert.AreEqual("0", response.Values["NumberOfPaymentsMade"].SingleOrDefault(), "Expected NumberOfPaymentsMade value is incorrect.");
			Assert.AreEqual("20", response.Values["RemainingNumberOfPayments"].SingleOrDefault(), "Expected RemainingNumberOfPayments value is incorrect.");
			Assert.AreEqual("14500.00", response.Values["TotalOutstandingAmount"].SingleOrDefault(), "Expected TotalOutstandingAmount value is incorrect.");
			Assert.AreEqual("10000.00", response.Values["OutstandingPrincipalAmount"].SingleOrDefault(), "Expected OutstandingPrincipalAmount value is incorrect.");
			Assert.AreEqual("500.00", response.Values["OutstandingFees"].SingleOrDefault(), "Expected OutstandingFees value is incorrect.");
			Assert.AreEqual("4000.00", response.Values["OutstandingInterest"].SingleOrDefault(), "Expected OutstandingInterest value is incorrect.");

            Drive.Data.Ops.SetServiceConfiguration<bool>(RiskBasedPricingEnabled, riskBasedPricingStatus);
		}

        [Test, AUT(AUT.Wb), JIRA("SME-251"), DependsOn("GetBusinessAccountSummary_ShouldReturnAccountDetails_WhenLoanIsInProgress")]
        public void GetBusinessAccountSummary_ShouldReturnAccountDetails_WhenLoanIsInProgressWhenRiskBasedPricingOff()
        {
            var riskBasedPricingStatus = Drive.Data.Ops.GetServiceConfiguration<bool>(RiskBasedPricingEnabled);
            Drive.Data.Ops.SetServiceConfiguration<bool>(RiskBasedPricingEnabled, !RiskBasedPricingStatusEnabled);

            var customer =
                CustomerBuilder.New().WithNumberOfDependants(0).WithGender(GenderEnum.Female).WithSpecificAge(40).Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanTerm(20).Build();

            Do.Until(() =>
                GetTransactionCount(
                t => application.Id == t.ApplicationEntity.ExternalId && t.Scope == (int)PaymentTransactionScopeEnum.Debit
                        && t.Type == PaymentTransactionEnum.Fee.ToString()));

            Do.With.Timeout(TimeSpan.FromSeconds(5)).Until(() =>
                GetTransactionCount(
                    t => application.Id == t.ApplicationEntity.ExternalId && t.Scope == (int)PaymentTransactionScopeEnum.Debit
                         && t.Type == PaymentTransactionEnum.Interest.ToString()));

            Do.With.Timeout(TimeSpan.FromSeconds(5)).Until(() =>
                GetTransactionCount(
                    t => application.Id == t.ApplicationEntity.ExternalId && t.Scope == (int)PaymentTransactionScopeEnum.Debit
                         && t.Type == PaymentTransactionEnum.CashAdvance.ToString()));

            Do.Until(() => Drive.Db.Payments.PaymentPlans.Single(pp => pp.ApplicationEntity.ExternalId == application.Id));

            var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
            {
                AccountId = customer.Id
            });

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Values["ApplicationId"]);
            Assert.AreEqual("true", response.Values["IsApplicationSigned"].SingleOrDefault(), "Expected IsApplicationSigned value is incorrect.");
            Assert.AreEqual("true", response.Values["IsApplicationAccepted"].SingleOrDefault(), "Expected IsApplicationAccepted value is incorrect.");
            Assert.AreEqual("0", response.Values["Arrears"].SingleOrDefault(), "Expected Arrears value is incorrect.");
            Assert.AreEqual("10000.00", response.Values["PrincipalLoanAmount"].SingleOrDefault(), "Expected PrincipalLoanAmount value is incorrect.");
            Assert.AreEqual("20", response.Values["LoanTerm"].SingleOrDefault(), "Expected LoanTerm value is incorrect.");
            Assert.AreEqual("565.00", response.Values["WeeklyRepaymentAmount"].SingleOrDefault(), "Expected WeeklyRepaymentAmount value is incorrect.");
            Assert.AreEqual("0", response.Values["NumberOfPaymentsMade"].SingleOrDefault(), "Expected NumberOfPaymentsMade value is incorrect.");
            Assert.AreEqual("20", response.Values["RemainingNumberOfPayments"].SingleOrDefault(), "Expected RemainingNumberOfPayments value is incorrect.");
            Assert.AreEqual("11300.00", response.Values["TotalOutstandingAmount"].SingleOrDefault(), "Expected TotalOutstandingAmount value is incorrect.");
            Assert.AreEqual("10000.00", response.Values["OutstandingPrincipalAmount"].SingleOrDefault(), "Expected OutstandingPrincipalAmount value is incorrect.");
            Assert.AreEqual("300.00", response.Values["OutstandingFees"].SingleOrDefault(), "Expected OutstandingFees value is incorrect.");
            Assert.AreEqual("1000.00", response.Values["OutstandingInterest"].SingleOrDefault(), "Expected OutstandingInterest value is incorrect.");

            Drive.Data.Ops.SetServiceConfiguration<bool>(RiskBasedPricingEnabled, riskBasedPricingStatus);
        }

		private int GetTransactionCount(Func<TransactionEntity, bool> func)
		{
			return Drive.Db.Payments.Transactions.Count(func);
		}
	}
}
