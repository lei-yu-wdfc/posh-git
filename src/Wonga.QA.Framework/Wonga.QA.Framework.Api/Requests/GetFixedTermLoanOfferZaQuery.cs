using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanOfferZa")]
    public partial class GetFixedTermLoanOfferZaQuery : ApiRequest<GetFixedTermLoanOfferZaQuery>
    {
        public Object AccountId { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
