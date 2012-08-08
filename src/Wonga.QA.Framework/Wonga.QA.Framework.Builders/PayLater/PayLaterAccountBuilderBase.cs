using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Builders.PayLater
{
	public abstract class PayLaterAccountBuilderBase
	{
		protected Guid AccountId { get; private set; }
		protected PayLaterAccountDataBase AccountData { get; private set; }


		protected PayLaterAccountBuilderBase(PayLaterAccountDataBase accountData) : this(Guid.NewGuid(), accountData){}

		protected PayLaterAccountBuilderBase(Guid accountId, PayLaterAccountDataBase accountData)
		{
			AccountId = accountId;
			AccountData = accountData;
		}

		public Customer Build()
		{
			CreateAccount();
			WaitUntilAccountIsPresentInServiceDatabases();

			return new Customer(AccountId);
		}

		protected void CreateAccount()
		{
			var commands = new List<ApiRequest>();

			commands.AddRange(GetGenericApiCommands());
			commands.AddRange(GetRegionSpecificApiCommands());

			Drive.Api.Commands.Post(commands);
		}

		protected IEnumerable<ApiRequest> GetGenericApiCommands()
		{
			throw new NotImplementedException();
		}

		abstract protected IEnumerable<ApiRequest> GetRegionSpecificApiCommands();

		private void WaitUntilAccountIsPresentInServiceDatabases()
		{
			throw new NotImplementedException();
		}

		#region "With" Methods - PersonalDetails
		#endregion
	}
}