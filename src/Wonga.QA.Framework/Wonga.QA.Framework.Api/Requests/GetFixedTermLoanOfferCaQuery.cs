using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanOffer")]
    public class GetFixedTermLoanOfferCaQuery : ApiRequest<GetFixedTermLoanOfferCaQuery>
    {
        public Object AccountId { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
