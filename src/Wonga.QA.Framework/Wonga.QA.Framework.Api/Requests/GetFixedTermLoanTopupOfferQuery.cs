using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanTopupOffer")]
    public partial class GetFixedTermLoanTopupOfferQuery : ApiRequest<GetFixedTermLoanTopupOfferQuery>
    {
        public Object AccountId { get; set; }
    }
}
