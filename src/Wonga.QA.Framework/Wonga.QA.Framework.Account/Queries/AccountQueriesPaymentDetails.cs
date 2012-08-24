using System;

namespace Wonga.QA.Framework.Account.Queries
{
	public class AccountQueriesPaymentDetails
	{
		public Guid GetPrimaryBankAccountGuid(Guid accountId)
		{
			var pDb = Drive.Data.Payments.Db;
			var bAccId = pDb.AccountPreferences.FindByAccountId(accountId).PrimaryBankAccountId;
			return pDb.BankAccountsBase.FindByBankAccountId(bAccId).ExternalId;
		}

		public Guid GetPrimaryPaymentCardGuid(Guid accountId)
		{
			var pDb = Drive.Data.Payments.Db;
			var pcId = pDb.AccountPreferences.FindByAccountId(accountId).PrimaryPaymentCardId;
			return pDb.PaymentCardsBase.FindByPaymentCardId(pcId).ExternalId;
		}
	}
}
