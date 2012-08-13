using System;

namespace Wonga.QA.Framework.Account.Queries
{
	public sealed class AccountQueriesDataPresence
	{
		public bool IsAccountPresentInServiceDatabases(AccountBase account)
		{
			return IsAccountPresentInServiceDatabases(account.Id);
		}

		public bool IsAccountPresentInServiceDatabases(Guid accountId)
		{
			return IsAccountPresentInOpsDatabase(accountId) &&
			       IsAccountPresentInCommsDatabase(accountId) &&
			       IsAccountPresentInPaymentsDatabase(accountId) &&
			       IsAccountPresentInRiskDatabase(accountId);
		}

		private bool IsAccountPresentInOpsDatabase(Guid accountId)
		{
			return Drive.Data.Ops.Db.Accounts.FindByExternalId(accountId).Any();
		}

		private bool IsAccountPresentInCommsDatabase(Guid accountId)
		{
			return Drive.Data.Comms.Db.CustomerDetails.FindByAccountId(accountId).Any();
		}

		private bool IsAccountPresentInPaymentsDatabase(Guid accountId)
		{
			return Drive.Data.Payments.Db.AccountPreferences.FindByAccountId(accountId).Any();
		}

		private bool IsAccountPresentInRiskDatabase(Guid accountId)
		{
			return Drive.Data.Risk.Db.RiskAccounts.FindByAccountId(accountId).Any();
		}
	}
}
