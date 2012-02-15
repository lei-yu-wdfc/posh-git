using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
	public class AddBankAccountTests
	{
		private static int _lastAccountNumber = 100000000;

		[Test, AUT(AUT.Ca), Parallelizable]
		public void AddBankAccountCa_Should_Add_Two_Accounts_Of_The_Same_Branch_InstituationNr()
		{
			Customer customer = CustomerBuilder.New().Build();

			AddBankAccountCaCommand defaultBankAccount = new AddBankAccountCaCommand();
			defaultBankAccount.Default();
			
			// Add another account at the branch of the default account created using the customer builder.
			AddBankAccountCaInternal(customer.Id, defaultBankAccount.InstitutionNumber.ToString(), defaultBankAccount.BranchNumber.ToString());

			Do.Until(() => Driver.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);
		}

		[Test, AUT(AUT.Ca), Parallelizable]
		public void AddBankAccountCa_Should_Add_Two_Accounts()
		{
			Customer customer = CustomerBuilder.New().Build();

			AddBankAccountCaInternal(customer.Id);

			Do.Until(() => Driver.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);
		}

		[Test, AUT(AUT.Ca), Parallelizable]
		[ExpectedException(typeof(Framework.Api.Exceptions.ValidatorException))]
		public void AddBankAccountCa_Should_Return_An_Error_When_Adding_The_3rd_Account()
		{
			Customer customer = CustomerBuilder.New().Build();

			AddBankAccountCaInternal(customer.Id);

			// Wait for view model to catch up, otherwise the validator in the API won't fire.
			Do.Until(() =>
			{
				var response = Driver.Api.Queries.Post(new GetBankAccountsQuery {AccountId = customer.Id});
				return response.Values.Contains("CanAddBankAccount") && "false" == response.Values["CanAddBankAccount"].Single();
			});

			AddBankAccountCaInternal(customer.Id);
		}

		[Test, AUT(AUT.Ca), Parallelizable]
		public void GetBankAccounts_should_return_the_account()
		{
			Customer customer = CustomerBuilder.New().Build();

			var request = new GetBankAccountsQuery
			{
				AccountId = customer.Id
			};

			var response = Driver.Api.Queries.Post(request);

			Assert.AreEqual(1, response.Values["BankCode"].Count());
			Assert.AreEqual(customer.GetBankAccount().ToString(), response.Values["BankAccountId"].Single());
		}

		[Test, AUT(AUT.Ca), Parallelizable]
		public void GetBankAccounts_ca_should_return_cannotaddbankaccounts_after_two_accounts_have_been_added()
		{
			Customer customer = CustomerBuilder.New().Build();

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
			string accountNumber = _lastAccountNumber++.ToString();

			AddBankAccountCaInternal(accountId, institutionNumber, branchNumber, accountNumber);
		}
	}
}
