using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.Ca.GetFixedTermLoanOffer </summary>
    [XmlRoot("GetFixedTermLoanOffer")]
    public partial class GetFixedTermLoanOfferCaQuery : ApiRequest<GetFixedTermLoanOfferCaQuery>
    {
        public Object AccountId { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
