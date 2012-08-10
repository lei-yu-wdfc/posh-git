using System;

namespace Wonga.QA.Framework.Account
{
	public static class AccountQueries
	{
		public static bool IsAccountPresentInServiceDatabases(AccountBase account)
		{
			return IsAccountPresentInServiceDatabases(account.Id);
		}

		public static bool IsAccountPresentInServiceDatabases(Guid accountId)
		{
			return IsAccountPresentInOpsDatabase(accountId) &&
				IsAccountPresentInCommsDatabase(accountId) &&
				IsAccountPresentInPaymentsDatabase(accountId) &&
				IsAccountPresentInRiskDatabase(accountId);
		}

		private static bool IsAccountPresentInOpsDatabase(Guid accountId)
		{
			return Drive.Data.Ops.Db.Accounts.FindByExternalId(accountId).Any();
		}

		private static bool IsAccountPresentInCommsDatabase(Guid accountId)
		{
			return Drive.Data.Comms.Db.CustomerDetails.FindByAccountId(accountId).Any();
		}

		private static bool IsAccountPresentInPaymentsDatabase(Guid accountId)
		{
			return Drive.Data.Payments.Db.AccountPreferences.FindByAccountId(accountId).Any();
		}

		private static bool IsAccountPresentInRiskDatabase(Guid accountId)
		{
			return Drive.Data.Risk.Db.RiskAccounts.FindByAccountId(accountId).Any();
		}
	}
}
