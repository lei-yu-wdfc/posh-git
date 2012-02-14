using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	public class AddBankAccountTests
	{
		[Test, AUT(AUT.Ca), Parallelizable]
		public void AddBankAccountCa_Should_Add_Two_Accounts()
		{
			Customer customer = CustomerBuilder.New().Build();

			AddBankAccountCaInternal(customer);

			Do.Until(() => Driver.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);
		}

		[Test, AUT(AUT.Ca), Parallelizable]
		[ExpectedException(typeof(Wonga.QA.Framework.Api.Exceptions.ValidatorException))]
		public void AddBankAccountCa_Should_Return_An_Error_When_Adding_The_3rd_Account()
		{
			Customer customer = CustomerBuilder.New().Build();

			AddBankAccountCaInternal(customer);

			Do.Until(() => Driver.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);

			AddBankAccountCaInternal(customer);
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

			// TODO: How can we parse the response back into the DTO?
			var response = Driver.Api.Queries.Post(request);
			Assert.AreEqual("true", response.Values["CanAddBankAccount"].Single());

			AddBankAccountCaInternal(customer);

			Do.Until(() => Driver.Db.Payments.BankAccountsBases.Count(a => a.PersonalBankAccountEntity.AccountId == customer.Id) == 2);

			response = Driver.Api.Queries.Post(request);
			Assert.AreEqual("false", response.Values["CanAddBankAccount"].Single());
		}

		private static void AddBankAccountCaInternal(Customer customer)
		{
			var requests = new List<ApiRequest>
    		               	{
    		               		AddBankAccountCaCommand.New(r =>
    		               		                            	{
    		               		                            		r.AccountId = customer.Id;
    		               		                            		r.AccountNumber = Environment.TickCount;
    		               		                            		r.InstitutionNumber = "001";
    		               		                            		r.BranchNumber = "01161";
    		               		                            	})
    		               	};

			Driver.Api.Commands.Post(requests);
		}
	}
}
