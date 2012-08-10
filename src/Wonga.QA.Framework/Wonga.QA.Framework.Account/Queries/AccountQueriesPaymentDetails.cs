using System;

namespace Wonga.QA.Framework.Account.Queries
{
	public class AccountQueriesPaymentDetails
	{
		public Guid GetPrimaryBankAccountGuid(AccountBase account)
		{
			var pDb = Drive.Data.Payments.Db;
			var bAccId = pDb.AccountPreferences.FindByAccountId(account.Id).PrimaryBankAccountId;
			return pDb.BankAccountsBase.FindByBankAccountId(bAccId).ExternalId;
		}

		public Guid GetPrimaryPaymentCardGuid(AccountBase account)
		{
			var pDb = Drive.Data.Payments.Db;
			var pcId = pDb.AccountPreferences.FindByAccountId(account.Id).PrimaryPaymentCardId;
			return pDb.PaymentCardsBase.FindByPaymentCardId(pcId).ExternalId;
		}
	}
}
