using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetFixedTermLoanTopupOffer </summary>
    [XmlRoot("GetFixedTermLoanTopupOffer")]
    public partial class GetFixedTermLoanTopupOfferQuery : ApiRequest<GetFixedTermLoanTopupOfferQuery>
    {
        public Object AccountId { get; set; }
    }
}
