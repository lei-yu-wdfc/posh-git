using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
	[XmlRoot("GetPendingPrepaidCardTransactions")]
	public partial class GetPendingPrepaidCardTransactionsQuery : ApiRequest<GetPendingPrepaidCardTransactionsQuery>
	{
		public Object AccountId { get; set; }
	}
}
