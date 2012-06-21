using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using AddBankAccountCaCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.Ca.AddBankAccountCaCommand;
using AddBankAccountUkCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk.AddBankAccountUkCommand;

namespace Wonga.QA.Tests.Payments
{
	[Parallelizable(TestScope.All)]
	public class AddBankAccountTests
	{
		#region Canada Tests

		private static int _lastAccountNumber = 100000000;

		[Test, AUT(AUT.Ca), JIRA("CA-1682")]
		public void AddBankAccountCaShouldAddTwoAccountsOfTheSameBranchInstituationNr()
		{
			Customer customer = CustomerBuilder.New().Build();

			var defaultBankAccount = new AddBankAccountCaCommand();
			defaultBankAccount.Default();

			// Add another account at the branch of the default account created using the customer builder.
			AddBankAccountCaInternal(customer.Id, defaultBankAccount.InstitutionNumber.ToString(), defaultBankAccount.BranchNumber.ToString());

			Do.Until(() => Drive.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-312")]
		public void AddBankAccountCaShouldAddTwoAccounts()
		{
			Customer customer = CustomerBuilder.New().Build();

			AddBankAccountCaInternal(customer.Id);

			Do.Until(() => Drive.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-312")]
		[ExpectedException(typeof(Framework.Api.Exceptions.ValidatorException))]
		public void AddBankAccountCaShouldReturnAnErrorWhenAddingThe3RdAccount()
		{
			Customer customer = CustomerBuilder.New().Build();

			AddBankAccountCaInternal(customer.Id);

			// Wait for view model to catch up, otherwise the validator in the API won't fire.
			Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(p => p.AccountId == customer.Id && !p.CanAddBankAccount));

			AddBankAccountCaInternal(customer.Id);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-312")]
		public void GetBankAccountsShouldReturnTheAccount()
		{
			Customer customer = CustomerBuilder.New().Build();

			Do.Until(customer.GetBankAccount);

			var request = new GetBankAccountsQuery
			{
				AccountId = customer.Id
			};

			var response = Drive.Api.Queries.Post(request);

			Assert.AreEqual(1, response.Values["BankCode"].Count());
			Assert.AreEqual(customer.GetBankAccount().ToString(), response.Values["BankAccountId"].Single());
		}


		[Test, AUT(AUT.Ca), JIRA("CA-312")]
		[Ignore("This has been rolled back and will be refactored as part of CA-1589")]
		public void GetBankAccountsCaShouldReturnCannotAddBankAccountsAfterTwoAccountsHaveBeenAdded()
		{
			Customer customer = CustomerBuilder.New().Build();

			Do.Until(customer.GetBankAccount);

			var request = new GetBankAccountsQuery
			{
				AccountId = customer.Id
			};

			var response = Drive.Api.Queries.Post(request);
			Assert.IsTrue(response.Values.Contains("CanAddBankAccount"));
			Assert.AreEqual("true", response.Values["CanAddBankAccount"].Single());

			AddBankAccountCaInternal(customer.Id);

			Do.Until(
				() => Drive.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);

			Do.Until(() =>
			{
				response = Drive.Api.Queries.Post(request);
				return response.Values.Contains("CanAddBankAccount") && "false" == response.Values["CanAddBankAccount"].Single();
			});
		}

		private static void AddBankAccountCaInternal(Guid accountId, string institutionNumber, string branchNumber, string accountNumber)
		{
			var requests = new List<ApiRequest>
    		               	{
    		               		AddBankAccountCaCommand.New(r =>
    		               		                            	{
    		               		                            		r.AccountId = accountId;
																	r.AccountNumber = accountNumber;
    		               		                            		r.InstitutionNumber = institutionNumber;
    		               		                            		r.BranchNumber = branchNumber;
    		               		                            	})
    		               	};

			Drive.Api.Commands.Post(requests);
		}

		private static void AddBankAccountCaInternal(Guid accountId, string institutionNumber = "001", string branchNumber = "01161")
		{
			string accountNumber = _lastAccountNumber++.ToString(CultureInfo.InvariantCulture);

			AddBankAccountCaInternal(accountId, institutionNumber, branchNumber, accountNumber);
		}

		#endregion

		#region Wb and UK Tests

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
		private static AddBankAccountUkCommand AddBankAccountWbInternal(Guid accountId, Boolean isPrimary, string accountNumber = "14690568", string sortCode = "309894")
		{
			return AddBankAccountUkCommand.New(x =>
			                                {
			                                    x.AccountId = accountId;
			                                    x.AccountNumber = accountNumber;
			                                    x.AccountOpenDate = DateTime.Now.AddMonths(-6);
			                                    x.BankAccountId = Guid.NewGuid();
			                                    x.BankCode = sortCode;
			                                    x.BankName = "HSBC";
			                                    x.CountryCode = "UK";
			                                    x.HolderName = "Test Holder";
			                                    x.IsPrimary = isPrimary;
			                                });
		}

		/// <summary>
		/// Scenario - Add primary bank account successfully
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add bank account details page
		/// When the applicant enters correct account information
		/// And external validation passes
		/// And this is the primary account
		/// Then the account should be created in the system
		/// And the account should be marked as primary
		/// And the payment card screen should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb, AUT.Uk), JIRA("SME-571", "SME-195"),CoreTest]
		public void AddPrimaryBankAccountSuccessfully()
		{
			var accountId = Guid.NewGuid();
			var message = AddBankAccountWbInternal(accountId, true);
			Drive.Api.Commands.Post(message);

			var db = Drive.Db.Payments;
			var baseBankAccountEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)message.BankAccountId && p.ValidatedOn != null));
			var accountPreferencesEntity = Do.Until(() => db.AccountPreferences.SingleOrDefault(p => p.AccountId == accountId));

			Assert.IsNotNull(baseBankAccountEntity, "Bank account base should not be null");
			Assert.IsNotNull(accountPreferencesEntity, "Account preferences should not be null");

			Assert.AreEqual(baseBankAccountEntity.ExternalId, message.BankAccountId);
			Assert.AreEqual(accountPreferencesEntity.AccountId, message.AccountId);

			Assert.AreEqual(baseBankAccountEntity.BankAccountId, accountPreferencesEntity.PrimaryBankAccountId);
			Assert.AreEqual(message.BankName, baseBankAccountEntity.BankName);
		}

		/// <summary>
		/// Scenario - Add secondary bank account successfully
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add bank account details page
		/// When the applicant enters correct account information
		/// And external validation passes
		/// And the applicant already has a primary account
		/// And this is not the primary account
		/// And maximum number of accounts has not been reached
		/// Then the account should be created in the system
		/// And the account should not be marked as primary
		/// And the payment card screen should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb, AUT.Uk), JIRA("SME-571", "SME-195")]
		public void AddSecondaryBankAccountSuccessfully()
		{
			var accountId = Guid.NewGuid();
			var addPrimaryBankAccountMessage = AddBankAccountWbInternal(accountId, true);
			var addSecondaryBankAccountMessage = AddBankAccountWbInternal(accountId, false, "63849203", "134020");

			Drive.Api.Commands.Post(addPrimaryBankAccountMessage);
			Drive.Api.Commands.Post(addSecondaryBankAccountMessage);

			var db = Drive.Db.Payments;
			var baseBankAccountPrimaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addPrimaryBankAccountMessage.BankAccountId && p.ValidatedOn != null));
			var baseBankAccountSecondaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addSecondaryBankAccountMessage.BankAccountId && p.ValidatedOn != null));
			var accountPreferencesEntity = Do.Until(() => db.AccountPreferences.SingleOrDefault(p => p.AccountId == accountId));

			Assert.IsNotNull(baseBankAccountPrimaryEntity);
			Assert.IsNotNull(baseBankAccountSecondaryEntity);
			Assert.IsNotNull(accountPreferencesEntity);

			Assert.AreEqual(accountPreferencesEntity.PrimaryBankAccountId, baseBankAccountPrimaryEntity.BankAccountId);
		}

		/// <summary>
		/// Scenario - Add another new primary bank account successfully
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add bank account details page
		/// And the main applicant has a primary account already
		/// When the applicant enters correct account information
		/// And external validation passes
		/// And this is the primary account
		/// And maximum number of accounts has not been reached
		/// Then the account should be created in the system
		/// And the new account should not be marked as primary
		/// And the existing primary account should be marked as secondary
		/// And the payment card screen should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb, AUT.Uk), JIRA("SME-571", "SME-195")]
		public void AddAnotherNewPrimaryBankAccountSuccessfully()
		{
			var accountId = Guid.NewGuid();
			var addPrimaryBankAccountMessage = AddBankAccountWbInternal(accountId, true);
			var addAnotherPrimaryBankAccountMessage = AddBankAccountWbInternal(accountId, true, "63849203", "134020");

			Drive.Api.Commands.Post(addPrimaryBankAccountMessage);
			var db = Drive.Db.Payments;
			var baseFirstBankAccountPrimaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addPrimaryBankAccountMessage.BankAccountId && p.ValidatedOn != null));

			Drive.Api.Commands.Post(addAnotherPrimaryBankAccountMessage);
			var baseSecondBankAccountPrimaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addAnotherPrimaryBankAccountMessage.BankAccountId && p.ValidatedOn != null));

			var accountPreferencesEntity = Do.Until(() => db.AccountPreferences.SingleOrDefault(p => p.AccountId == accountId && p.PrimaryBankAccountId == baseSecondBankAccountPrimaryEntity.BankAccountId));
            
			Assert.IsNotNull(baseFirstBankAccountPrimaryEntity);
			Assert.IsNotNull(baseSecondBankAccountPrimaryEntity);
			Assert.IsNotNull(accountPreferencesEntity);
		}

		/// <summary>
		/// Scenario - Incorrect sort code length
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add bank account details page
		/// When the applicant enters sort code with length other than 6 characters
		/// Then an error message should be displayed
		/// </scenario>
		[Test, AUT(AUT.Wb, AUT.Uk), JIRA("SME-571", "SME-195")]
		public void IncorrectSortCodeLength()
		{
			var accountId = Guid.NewGuid();
			var addPrimaryBankAccountMessage = AddBankAccountWbInternal(accountId, true, "63849203", "1");

			Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(addPrimaryBankAccountMessage), "Payments_BankCode_InvalidLength");
		}

		/// <summary>
		/// Scenario -  Incorrect account number length
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add bank account details page
		/// When the applicant enters account number with length other than 8 characters
		/// Then an error message should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb, AUT.Uk), JIRA("SME-571", "SME-195")]
		public void IncorrectAccountNumberLength()
		{
			var accountId = Guid.NewGuid();
			var addPrimaryBankAccountMessage = AddBankAccountWbInternal(accountId, true, "1");

			Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(addPrimaryBankAccountMessage), "Payments_AccountNumber_InvalidLength");
		}

		/// <summary>
		/// Scenario -  Duplicate account
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add bank account details page
		/// When the applicant enters an existing account information
		/// Then an error message should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb, AUT.Uk), JIRA("SME-571", "SME-195")]
		public void DuplicateAccount()
		{
			var accountId = Guid.NewGuid();
			var addPrimaryBankAccountMessage = AddBankAccountWbInternal(accountId, true);
			var addAnotherPrimaryBankAccountMessage = AddBankAccountWbInternal(accountId, true);

			Drive.Api.Commands.Post(addPrimaryBankAccountMessage);

			var db = Drive.Db.Payments;
			var baseFirstBankAccountPrimaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addPrimaryBankAccountMessage.BankAccountId && p.ValidatedOn != null));
			Assert.IsNotNull(baseFirstBankAccountPrimaryEntity);

			Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(addAnotherPrimaryBankAccountMessage), "Payments_BankAccount_DuplicateBankAccount");
		}

		/// <summary>
		/// Scenario -  Max number of accounts reached
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add bank account details page
		/// When the applicant enters correct account information
		/// And applicant already has 5 accounts
		/// Then an error message should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb, AUT.Uk), JIRA("SME-571", "SME-195")]
		public void MaximumNumberOfAccountsReached()
		{
			var accountId = Guid.NewGuid();

			var addFirstBankAccountMessage = AddBankAccountWbInternal(accountId, true);
			var addSecondBankAccountMessage = AddBankAccountWbInternal(accountId, false, "63849203", "134020");
			var addThirdBankAccountMessage = AddBankAccountWbInternal(accountId, false, "07806039", "938611");
			var addFourthBankAccountMessage = AddBankAccountWbInternal(accountId, false, "42368003", "938600");
			var addFifthBankAccountMessage = AddBankAccountWbInternal(accountId, false, "06774744", "086090");

			var addSixthBankAccountMessage = AddBankAccountWbInternal(accountId, false, "00000190", "180002");

			Drive.Api.Commands.Post(addFirstBankAccountMessage);
			Drive.Api.Commands.Post(addSecondBankAccountMessage);
			Drive.Api.Commands.Post(addThirdBankAccountMessage);
			Drive.Api.Commands.Post(addFourthBankAccountMessage);
			Drive.Api.Commands.Post(addFifthBankAccountMessage);

			var db = Drive.Db.Payments;
			var baseFirstBankAccountPrimaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addFirstBankAccountMessage.BankAccountId && p.ValidatedOn != null));
			var baseSecondBankAccountPrimaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addSecondBankAccountMessage.BankAccountId && p.ValidatedOn != null));
			var baseThirdBankAccountPrimaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addFirstBankAccountMessage.BankAccountId && p.ValidatedOn != null));
			var baseFourthBankAccountPrimaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addSecondBankAccountMessage.BankAccountId && p.ValidatedOn != null));
			var baseFifthBankAccountPrimaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addFirstBankAccountMessage.BankAccountId && p.ValidatedOn != null));

			var accountPreferencesEntity = Do.Until(() => db.AccountPreferences.SingleOrDefault(p => p.AccountId == accountId));

			Assert.IsNotNull(baseFirstBankAccountPrimaryEntity);
			Assert.IsNotNull(baseSecondBankAccountPrimaryEntity);
			Assert.IsNotNull(baseThirdBankAccountPrimaryEntity);
			Assert.IsNotNull(baseFourthBankAccountPrimaryEntity);
			Assert.IsNotNull(baseFifthBankAccountPrimaryEntity);
			Assert.IsNotNull(accountPreferencesEntity);

			Assert.AreEqual(accountPreferencesEntity.PrimaryBankAccountId, baseFirstBankAccountPrimaryEntity.BankAccountId);

			// now add one too much
			Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(addSixthBankAccountMessage), "Payments_BankAccount_MaxNumberReached");
		}

		/// <summary>
		/// Scenario - External validation fails
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add bank account details page
		/// When the applicant enters account information
		/// And external validation fails
		/// Then the account should be created in the system
		/// And the account should be marked as deactivated (could not find any services that would handle IBankAccountDeactivated)
		/// And the account should be marked as invalid
		/// And the payment card screen should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb, AUT.Uk), JIRA("SME-571", "SME-195")]
		public void ExternalValidationFails()
		{
			var accountId = Guid.NewGuid();
			var message = AddBankAccountWbInternal(accountId, true, "66666666", "666666");
			Drive.Api.Commands.Post(message);

			var db = Drive.Db.Payments;
			var baseBankAccountEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)message.BankAccountId && p.ValidateFailedOn != null));

			Assert.IsNotNull(baseBankAccountEntity, "Bank account base should not be null");
			Assert.AreEqual(baseBankAccountEntity.ExternalId, message.BankAccountId);
			Assert.AreEqual(message.BankName, baseBankAccountEntity.BankName);
		}

		/// <summary>
		/// Scenario - External validation passes
		/// </summary>
		/// <scenario>
		/// Given the main applicant is on the add bank account details page
		/// When the applicant enters account information
		/// And external validation passes
		/// Then the account should be created in the system
		/// And the account should be marked as activated (could not find any service that would handle IBankAccountActivated)
		/// And the account should be marked as valid
		/// And the payment card screen should be displayed 
		/// </scenario>
		[Test, AUT(AUT.Wb, AUT.Uk), JIRA("SME-571", "SME-195")]
		public void ExternalValidationPasses()
		{
			var accountId = Guid.NewGuid();
			var message = AddBankAccountWbInternal(accountId, true);
			Drive.Api.Commands.Post(message);

			var db = Drive.Db.Payments;
			var baseBankAccountEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)message.BankAccountId && p.ValidatedOn != null));

			Assert.IsNotNull(baseBankAccountEntity, "Bank account base should not be null");
			Assert.AreEqual(baseBankAccountEntity.ExternalId, message.BankAccountId);
		}

		#endregion
	}
}
