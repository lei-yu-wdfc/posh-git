using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetApplicationApr")]
	public partial class GetApplicationAprUkQuery : ApiRequest<GetApplicationAprUkQuery>
	{
		public Object MerchantId { get; set; }
		public Object TotalValue { get; set; }
		public Object PromoCodeId { get; set; }
	}
}
