using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetPayLaterTransactions")]
	public partial class GetPayLaterTransactionsUkQuery : ApiRequest<GetPayLaterTransactionsUkQuery>
	{
		public Object AccountId { get; set; }
	}
}
