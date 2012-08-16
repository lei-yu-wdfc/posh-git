using System;

namespace Wonga.QA.Framework.Application.Queries.Consumer
{
	public sealed class ConsumerApplicationQueries
	{
		public Guid GetAccountIdForApplication(Guid applicationId)
		{
			return (Guid)Drive.Data.Payments.Db.Applications.FindByExternalId(applicationId).AccountId;
		}
	}
}
