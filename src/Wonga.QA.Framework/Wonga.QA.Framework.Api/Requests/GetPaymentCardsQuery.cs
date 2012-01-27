using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPaymentCards")]
    public class GetPaymentCardsQuery : ApiRequest<GetPaymentCardsQuery>
    {
        public Object AccountId { get; set; }
    }
}
