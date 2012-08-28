using System;
using System.Data;
using System.Linq;
using Wonga.QA.Framework.Core;

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
        	try
        	{
				return Drive.Db.Ops.Accounts.Any(a => a.ExternalId == accountId);
        	}
        	catch (DoException)
        	{
        		throw new DataException(String.Format("Ops Account not present for AccountId: {0}", accountId));
        	}
		}

        public bool IsAccountPresentInCommsDatabase(Guid accountId)
		{
        	try
        	{
				return Drive.Db.Comms.CustomerDetails.Any(a => a.AccountId == accountId);
        	}
        	catch (DoException)
        	{
				throw new DataException(String.Format("Comms CustomerDetails not present for AccountId: {0}", accountId));
        	}
		}

        public bool IsAccountPresentInPaymentsDatabase(Guid accountId)
		{
        	try
        	{
				return Drive.Db.Payments.AccountPreferences.Any(a => a.AccountId == accountId);
        	}
        	catch (DoException)
        	{
				throw new DataException(String.Format("Payments AccountPreferences not present for AccountId: {0}", accountId));
        	}
		}

        public bool IsAccountPresentInRiskDatabase(Guid accountId)
		{
        	try
        	{
				return Drive.Db.Risk.RiskAccounts.Any(a => a.AccountId == accountId);
        	}
        	catch (DoException)
        	{
				throw new DataException(String.Format("Risk Account not present for AccountId: {0}", accountId));
        	}
		}
	}
}
