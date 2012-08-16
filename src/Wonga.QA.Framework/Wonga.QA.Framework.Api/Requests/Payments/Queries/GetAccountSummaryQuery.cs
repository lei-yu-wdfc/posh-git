using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetAccountSummary")]
	public partial class GetAccountSummaryQuery : ApiRequest<GetAccountSummaryQuery>
	{
		public Object AccountId { get; set; }
	}
}
