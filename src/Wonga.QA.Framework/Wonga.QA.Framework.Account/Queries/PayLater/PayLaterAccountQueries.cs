﻿using System;
using System.Linq;
using Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk;

namespace Wonga.QA.Framework.Account.Queries.PayLater
{
	public sealed class PayLaterAccountQueries : AccountQueriesBase
	{
		public Decimal AvailableCredit(Guid accountId)
		{
			var response = Drive.Api.Queries.Post(new GetAvailableCreditPayLaterUkQuery() {AccountId = accountId.ToString()});
			return Decimal.Parse(response.Values["AvailableCredit"].Single());
		}
	}
}
