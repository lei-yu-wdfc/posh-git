using System;
using System.Linq;

namespace Wonga.QA.Framework.Account.Queries
{
	public sealed class AccountQueriesDataPresence
	{
		public bool IsAccountPresentInServiceDatabases(Guid accountId)
		{
			return IsAccountPresentInOpsDatabase(accountId) &&
			       IsAccountPresentInCommsDatabase(accountId) &&
			       IsAccountPresentInPaymentsDatabase(accountId) &&
			       IsAccountPresentInRiskDatabase(accountId);
		}

        public bool IsAccountPresentInOpsDatabase(Guid accountId)
		{
            return Drive.Db.Ops.Accounts.Any(a => a.ExternalId == accountId);
		}

        public bool IsAccountPresentInCommsDatabase(Guid accountId)
		{
            return Drive.Db.Comms.CustomerDetails.Any(a => a.AccountId == accountId);
		}

        public bool IsAccountPresentInPaymentsDatabase(Guid accountId)
		{
            return Drive.Db.Payments.AccountPreferences.Any(a => a.AccountId == accountId);
		}

        public bool IsAccountPresentInRiskDatabase(Guid accountId)
		{
            return Drive.Db.Risk.RiskAccounts.Any(a => a.AccountId == accountId);
		}
	}
}
