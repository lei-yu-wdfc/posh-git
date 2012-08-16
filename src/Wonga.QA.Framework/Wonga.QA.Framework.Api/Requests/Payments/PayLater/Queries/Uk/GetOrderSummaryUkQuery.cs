using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetOrderSummary")]
	public partial class GetOrderSummaryUkQuery : ApiRequest<GetOrderSummaryUkQuery>
	{
		public Object MerchantReference { get; set; }
		public Object MerchantOrderId { get; set; }
	}
}
