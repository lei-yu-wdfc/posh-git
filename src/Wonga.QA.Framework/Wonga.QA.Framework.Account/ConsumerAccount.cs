using System;
using Wonga.QA.Framework.Account.Queries;

namespace Wonga.QA.Framework.Account
{
	public class ConsumerAccount : WongaAccount
	{
		public ConsumerAccount(Guid accountId) : base(accountId)
		{
		}

		public Decimal AvailablePayLaterCredit
		{
			get { return AccountQueries.PayLater.AvailableCredit(Id); }
		}
	}
}
