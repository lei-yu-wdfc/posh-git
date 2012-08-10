using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Account;
using Wonga.QA.Framework.Account.PayLater;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Ops.Commands;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.PayLater
{
	public abstract class PayLaterAccountBuilderBase
	{
		protected Guid AccountId { get; private set; }
		protected Guid PrimaryPhoneVerificationId { get; private set; }
		protected PayLaterAccountDataBase AccountData { get; private set; }


		protected PayLaterAccountBuilderBase(PayLaterAccountDataBase accountData) : this(Guid.NewGuid(), accountData){}

		protected PayLaterAccountBuilderBase(Guid accountId, PayLaterAccountDataBase accountData)
		{
			AccountId = accountId;
			AccountData = accountData;
		}

		public PayLaterAccount Build()
		{
			CreateAccount();
			WaitUntilAccountIsPresentInServiceDatabases();
			return new PayLaterAccount(AccountId);
		}

		protected void CreateAccount()
		{
			var commands = new List<ApiRequest>();
			commands.AddRange(GetGenericApiCommands());
			Drive.Api.Commands.Post(commands);
		}

		protected IEnumerable<ApiRequest> GetGenericApiCommands()
		{
			yield return new CreateAccountCommand
			             	{
			             		AccountId = this.AccountId,
			             		Login = AccountData.Email,
			             		Password = AccountData.Password
			             	};
		}

		abstract protected IEnumerable<ApiRequest> GetRegionSpecificApiCommands();

		private void WaitUntilAccountIsPresentInServiceDatabases()
		{
			Do.Until(() =>  AccountQueries.IsAccountPresentInServiceDatabases(AccountId));
		}

		#region "With" Methods - PersonalDetails
		#endregion
	}
}