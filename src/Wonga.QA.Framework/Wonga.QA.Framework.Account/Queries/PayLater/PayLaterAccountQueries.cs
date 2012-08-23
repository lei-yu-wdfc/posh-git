using System;
using Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk;

namespace Wonga.QA.Framework.Account.Queries.PayLater
{
	public sealed class PayLaterAccountQueries : AccountQueriesBase
	{
		public Decimal GetTrustRating(Guid accountId)
		{
			var response = Drive.Api.Queries.Post(new GetAvailableCreditPayLaterUkQuery() {AccountId = accountId});
			return Decimal.Parse(response.Values["AvailableCredit"].ToString());
		}
	}
}
