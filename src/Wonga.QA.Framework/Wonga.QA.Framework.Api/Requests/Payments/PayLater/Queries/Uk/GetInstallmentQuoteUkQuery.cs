using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetInstallmentQuote")]
	public partial class GetInstallmentQuoteUkQuery : ApiRequest<GetInstallmentQuoteUkQuery>
	{
		public Object MerchantId { get; set; }
		public Object TotalValue { get; set; }
		public Object PromoCodeId { get; set; }
	}
}
