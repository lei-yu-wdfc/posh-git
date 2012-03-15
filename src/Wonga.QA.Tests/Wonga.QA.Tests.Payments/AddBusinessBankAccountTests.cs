using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[Parallelizable(TestScope.All)]
	public class AddBusinessBankAccountTests
	{
		/// <summary>
		/// Adds the bank account wb internal.
		/// Other valid sort codes / acc numbers:
		/// 134020 / 63849203
		/// 938611 / 07806039
		/// 938600 / 42368003
		/// 086090 / 06774744
		/// 074456 / 12345112
		/// 070116 / 34012583
		/// 180002 / 00000190
		/// </summary>
		private static AddBusinessBankAccountWbUkCommand AddBusinessBankAccountWbInternal(Guid organisationId, string accountNumber = "14690568", string sortCode = "309894")
		{
			return new AddBusinessBankAccountWbUkCommand()
			{
				OrganisationId = organisationId,
				AccountNumber = accountNumber,
				AccountOpenDate = DateTime.Now.AddMonths(-6),
				BankAccountId = Guid.NewGuid(),
				BankCode = sortCode,
				BankName = "HSBC",
				CountryCode = "UK",
				HolderName = "Test Holder"
			};
		}

		/// <summary>
		/// SME-207 https://jira.wonga.com/browse/SME-207
		/// </summary>
		/// <summary>
		/// Scenario – Add business bank account successfully 
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add business bank account details page
		/// When the applicant enters correct account information
		/// And external validation passes
		/// Then the account should be created in the system
		/// And the business payment card screen should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb), JIRA("SME-207")]
		public void AddBusinessBankAccountSuccessfully()
		{
			var orgId = Guid.NewGuid();
			var message = AddBusinessBankAccountWbInternal(orgId);
			Driver.Api.Commands.Post(message);

			var db = Driver.Db.Payments;
			var baseBankAccountEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)message.BankAccountId && p.ValidatedOn != null));
			var businessBankAccountEntity = Do.Until(() => db.BusinessBankAccounts.SingleOrDefault(p => p.BankAccountId == baseBankAccountEntity.BankAccountId));

			Assert.IsNotNull(baseBankAccountEntity, "Bank account base should not be null");
			Assert.IsNotNull(businessBankAccountEntity, "Business bank account should not be null");

			Assert.AreEqual(baseBankAccountEntity.ExternalId, message.BankAccountId);
			Assert.AreEqual(message.BankName, baseBankAccountEntity.BankName);
		}

		/// <summary>
		/// Scenario – Incorrect sort code length 
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add business bank account details page
		/// When the applicant enters sort code with length other than 6 characters
		/// Then an error message should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb), JIRA("SME-207")]
		public void IncorrectSortCodeLength()
		{
			var orgId = Guid.NewGuid();
			var message = AddBusinessBankAccountWbInternal(orgId, sortCode: "1");

			Assert.Throws<ValidatorException>(() => Driver.Api.Commands.Post(message), "Payments_BankCode_InvalidLength");
		}

		/// <summary>
		/// Scenario – Incorrect account number length 
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add business bank account details page
		/// When the applicant enters account number with length other than 8 characters
		/// Then an error message should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb), JIRA("SME-207")]
		public void IncorrectAccountNumberLength()
		{
			var orgId = Guid.NewGuid();
			var message = AddBusinessBankAccountWbInternal(orgId, accountNumber: "1");

			Assert.Throws<ValidatorException>(() => Driver.Api.Commands.Post(message), "Payments_AccountNumber_InvalidLength");
		}

		/// <summary>
		/// Scenario – Max number of accounts reached 
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add business bank account details page
		/// When the applicant enters correct account information
		/// And applicant already has 1 account
		/// Then an error message should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb), JIRA("SME-207")]
		public void MaximumNumberOfAccountsReached()
		{
			var orgId = Guid.NewGuid();
			var firstAccount = AddBusinessBankAccountWbInternal(orgId);
			var secondAccount = AddBusinessBankAccountWbInternal(orgId);
			Driver.Api.Commands.Post(firstAccount);

			var db = Driver.Db.Payments;
			var baseBankAccountEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)firstAccount.BankAccountId && p.ValidatedOn != null));
			var businessBankAccountEntity = Do.Until(() => db.BusinessBankAccounts.SingleOrDefault(p => p.BankAccountId == baseBankAccountEntity.BankAccountId));

			Assert.IsNotNull(baseBankAccountEntity, "Bank account base should not be null");
			Assert.IsNotNull(businessBankAccountEntity, "Business bank account should not be null");

			Assert.AreEqual(baseBankAccountEntity.ExternalId, firstAccount.BankAccountId);
			Assert.AreEqual(firstAccount.BankName, baseBankAccountEntity.BankName);

			Assert.Throws<ValidatorException>(() => Driver.Api.Commands.Post(secondAccount), "Payments_BankAccount_MaxNumberReached");
		}

		/// <summary>
		/// Scenario – External validation fails 
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add business bank account details page
		/// When the applicant enters account information
		/// And external validation fails
		/// Then the account should be created in the system
		/// And the account should be marked as invalid
		/// And the business payment card screen should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb), JIRA("SME-207")]
		public void ExternalValidationFails()
		{
			var orgId = Guid.NewGuid();
			var message = AddBusinessBankAccountWbInternal(orgId, "66666666", "666666");
			Driver.Api.Commands.Post(message);

			var db = Driver.Db.Payments;
			var baseBankAccountEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)message.BankAccountId && p.ValidateFailedOn != null));
			var businessBankAccountEntity = Do.Until(() => db.BusinessBankAccounts.SingleOrDefault(p => p.BankAccountId == baseBankAccountEntity.BankAccountId));

			Assert.IsNotNull(baseBankAccountEntity, "Bank account base should not be null");
			Assert.IsNotNull(businessBankAccountEntity, "Business bank account should not be null");

			Assert.AreEqual(baseBankAccountEntity.ExternalId, message.BankAccountId);
			Assert.AreEqual(message.BankName, baseBankAccountEntity.BankName);
		}

		/// <summary>
		/// Scenario – External validation passes 
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the business add bank account details page
		/// When the applicant enters account information
		/// And external validation passes
		/// Then the account should be created in the system
		/// And the account should be marked as valid
		/// And the business payment card screen should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb), JIRA("SME-207")]
		public void ExternalValidationPasses()
		{
			var orgId = Guid.NewGuid();
			var message = AddBusinessBankAccountWbInternal(orgId);
			Driver.Api.Commands.Post(message);

			var db = Driver.Db.Payments;
			var baseBankAccountEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)message.BankAccountId && p.ValidatedOn != null));
			var businessBankAccountEntity = Do.Until(() => db.BusinessBankAccounts.SingleOrDefault(p => p.BankAccountId == baseBankAccountEntity.BankAccountId));

			Assert.IsNotNull(baseBankAccountEntity, "Bank account base should not be null");
			Assert.IsNotNull(businessBankAccountEntity, "Business bank account should not be null");
		}
	}
}