using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Za
{
    /// <summary> Wonga.Payments.Queries.Za.GetFixedTermLoanOfferZa </summary>
    [XmlRoot("GetFixedTermLoanOfferZa")]
    public partial class GetFixedTermLoanOfferZaQuery : ApiRequest<GetFixedTermLoanOfferZaQuery>
    {
        public Object AccountId { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
