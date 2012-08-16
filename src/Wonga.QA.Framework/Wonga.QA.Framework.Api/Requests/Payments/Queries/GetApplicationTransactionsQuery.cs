using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetApplicationTransactions")]
	public partial class GetApplicationTransactionsQuery : ApiRequest<GetApplicationTransactionsQuery>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
	}
}
