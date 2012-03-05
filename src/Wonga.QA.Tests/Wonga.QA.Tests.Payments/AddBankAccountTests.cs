using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using AddBankAccountCaCommand = Wonga.QA.Framework.Api.AddBankAccountCaCommand;

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

            Do.Until(() => Driver.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-312")]
        public void AddBankAccountCaShouldAddTwoAccounts()
        {
            Customer customer = CustomerBuilder.New().Build();

            AddBankAccountCaInternal(customer.Id);

            Do.Until(() => Driver.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-312")]
        [ExpectedException(typeof(Framework.Api.Exceptions.ValidatorException))]
        public void AddBankAccountCaShouldReturnAnErrorWhenAddingThe3RdAccount()
        {
            Customer customer = CustomerBuilder.New().Build();

            AddBankAccountCaInternal(customer.Id);

            // Wait for view model to catch up, otherwise the validator in the API won't fire.
            Do.Until(() => Driver.Db.Payments.AccountPreferences.Single(p => p.AccountId == customer.Id && !p.CanAddBankAccount));

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

            var response = Driver.Api.Queries.Post(request);

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

            var response = Driver.Api.Queries.Post(request);
            Assert.IsTrue(response.Values.Contains("CanAddBankAccount"));
            Assert.AreEqual("true", response.Values["CanAddBankAccount"].Single());

            AddBankAccountCaInternal(customer.Id);

            Do.Until(
                () => Driver.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);

            Do.Until(() =>
            {
                response = Driver.Api.Queries.Post(request);
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

            Driver.Api.Commands.Post(requests);
        }
        private static void AddBankAccountCaInternal(Guid accountId, string institutionNumber = "001", string branchNumber = "01161")
        {
            string accountNumber = _lastAccountNumber++.ToString(CultureInfo.InvariantCulture);

            AddBankAccountCaInternal(accountId, institutionNumber, branchNumber, accountNumber);
        }
        private static AddBankAccountUkCommand AddBankAccountWbInternal(Guid accountId, Boolean isPrimary)
        {
            return new AddBankAccountUkCommand()
                              {
                                  AccountId = accountId,
                                  AccountNumber = "14690568",
                                  AccountOpenDate = DateTime.Now.AddMonths(-6),
                                  BankAccountId = Guid.NewGuid(),
                                  BankCode = "309894",
                                  BankName = "HSBC",
                                  CountryCode = "UK",
                                  HolderName = "Test Holder",
                                  IsPrimary = isPrimary,
                              };
        }

        #endregion

        #region Wb Tests

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
        [Test,AUT(AUT.Wb),JIRA("SME-571")]
        public void AddPrimaryBankAccountSuccessfully()
        {
            var accountId = Guid.NewGuid();
            var message = AddBankAccountWbInternal(accountId, true);
            Driver.Api.Commands.Post(message);

            var db = Driver.Db.Payments;
            var baseBankAccountEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)message.BankAccountId));
            var accountPreferencesEntity = Do.Until(() => db.AccountPreferences.SingleOrDefault(p => p.AccountId == accountId));

            Assert.IsNotNull(baseBankAccountEntity,"Bank account base should not be null");
            Assert.IsNotNull(accountPreferencesEntity,"Account preferences should not be null");

            Assert.AreEqual(baseBankAccountEntity.ExternalId,message.BankAccountId);
            Assert.AreEqual(accountPreferencesEntity.AccountId,message.AccountId);

            Assert.AreEqual(baseBankAccountEntity.BankAccountId, accountPreferencesEntity.PrimaryBankAccountId);
            Assert.AreEqual(message.BankName,baseBankAccountEntity.BankName);
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
        [Test, AUT(AUT.Wb), JIRA("SME-571")]
        [Pending("Not finished yet!")]
        public void AddSecondaryBankAccountSuccessfully()
        {
            var accountId = Guid.NewGuid();
            var addPrimaryBankAccountMessage = AddBankAccountWbInternal(accountId, true);
            var addSecondaryBankAccountMessage = AddBankAccountWbInternal(accountId, false);
            addSecondaryBankAccountMessage.BankCode = "110292";


            Driver.Api.Commands.Post(addPrimaryBankAccountMessage);
            Driver.Api.Commands.Post(addSecondaryBankAccountMessage);

            var db = Driver.Db.Payments;
            var baseBankAccountPrimaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addPrimaryBankAccountMessage.BankAccountId));
            var baseBankAccountSecondaryEntity = Do.Until(() => db.BankAccountsBases.SingleOrDefault(p => p.ExternalId == (Guid)addSecondaryBankAccountMessage.BankAccountId));
            var accountPreferencesEntity = Do.Until(() => db.AccountPreferences.SingleOrDefault(p => p.AccountId == accountId));

            Assert.IsNotNull(baseBankAccountPrimaryEntity);
            Assert.IsNotNull(baseBankAccountSecondaryEntity);
            Assert.IsNotNull(accountPreferencesEntity);

            Assert.AreEqual(accountPreferencesEntity.PrimaryBankAccountId,baseBankAccountPrimaryEntity.BankAccountId);
        }

        #endregion

    }
}
