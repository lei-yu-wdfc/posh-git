using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanOffer")]
    public partial class GetFixedTermLoanOfferUkQuery : ApiRequest<GetFixedTermLoanOfferUkQuery>
    {
        public Object AccountId { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
