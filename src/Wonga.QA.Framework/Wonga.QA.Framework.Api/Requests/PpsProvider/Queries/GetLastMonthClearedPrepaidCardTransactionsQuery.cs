using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
	[XmlRoot("GetLastMonthClearedPrepaidCardTransactions")]
	public partial class GetLastMonthClearedPrepaidCardTransactionsQuery : ApiRequest<GetLastMonthClearedPrepaidCardTransactionsQuery>
	{
		public Object AccountId { get; set; }
	}
}
