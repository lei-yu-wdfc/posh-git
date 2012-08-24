
using System;

namespace Wonga.QA.Framework.Application.Queries
{
	public abstract class ApplicationQueriesBase
	{
		public Guid GetAccountGuidForApplication(Guid applicationId)
		{
			return (Guid)Drive.Data.Payments.Db.Applications.FindByExternalId(applicationId).AccountId;
		}

		public abstract Decimal GetAmountToRepay(Guid applicationId);
	}
}
