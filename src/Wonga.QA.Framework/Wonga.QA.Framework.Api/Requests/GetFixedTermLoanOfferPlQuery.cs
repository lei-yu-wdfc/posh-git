using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.Pl.GetFixedTermLoanOffer </summary>
    [XmlRoot("GetFixedTermLoanOffer")]
    public partial class GetFixedTermLoanOfferPlQuery : ApiRequest<GetFixedTermLoanOfferPlQuery>
    {
        public Object AccountId { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
