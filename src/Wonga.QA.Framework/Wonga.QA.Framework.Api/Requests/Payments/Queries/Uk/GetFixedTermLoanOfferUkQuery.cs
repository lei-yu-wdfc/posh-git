using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk
{
	[XmlRoot("GetFixedTermLoanOffer")]
	public partial class GetFixedTermLoanOfferUkQuery : ApiRequest<GetFixedTermLoanOfferUkQuery>
	{
		public Object AccountId { get; set; }
		public Object PromoCodeId { get; set; }
        public Object TransmissionFeeDiscount { get; set; }
	}
}
