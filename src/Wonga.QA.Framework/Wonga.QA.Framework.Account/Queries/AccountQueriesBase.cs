using System;

namespace Wonga.QA.Framework.Account.Queries
{
	public abstract class AccountQueriesBase
	{
		public AccountQueriesDataPresence DataPresence = new AccountQueriesDataPresence();
		public AccountQueriesPaymentDetails PaymentDetails = new AccountQueriesPaymentDetails();
        public AccountQueriesCustomerDetails CustomerDetails = new AccountQueriesCustomerDetails();
	}
}
