using System;
using Wonga.QA.Framework.Account.Queries;

namespace Wonga.QA.Framework.Account
{
	public class PayLaterAccount : AccountBase
	{
		public PayLaterAccount(Guid accountId) : base(accountId){}

		#region Queries

		public Decimal TrustRating
		{
			get { return AccountQueries.PayLater.GetTrustRating(Id); }
		}

		#endregion

		#region Operations

		#endregion
	}
}
