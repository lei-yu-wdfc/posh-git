using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetPromoCodeParameters")]
	public partial class GetPromoCodeParametersQuery : ApiRequest<GetPromoCodeParametersQuery>
	{
		public Object AccountId { get; set; }
		public Object AffiliateId { get; set; }
		public Object PromoCode { get; set; }
	}
}
