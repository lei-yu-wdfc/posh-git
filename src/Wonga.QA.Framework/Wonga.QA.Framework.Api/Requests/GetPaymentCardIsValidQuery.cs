using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPaymentCardIsValid")]
    public class GetPaymentCardIsValidQuery : ApiRequest<GetPaymentCardIsValidQuery>
    {
        public Object PaymentCardId { get; set; }
    }
}
