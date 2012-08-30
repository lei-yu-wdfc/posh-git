
using System;
using Wonga.QA.Framework.Account.Queries;

namespace Wonga.QA.Framework.Account
{
	public abstract class WongaAccount : AccountBase
	{
		protected WongaAccount(Guid accountId) : base(accountId){}

		public Decimal ConsumerCredit
		{
			get { return AccountQueries.Consumer.AvailableCredit(Id); }
		}
		public Decimal PayLaterCredit
		{
			get { return AccountQueries.PayLater.AvailableCredit(Id); }
		}

	}
}
