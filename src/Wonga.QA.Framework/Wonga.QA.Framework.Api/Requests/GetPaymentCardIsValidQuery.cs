using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPaymentCardIsValid")]
    public partial class GetPaymentCardIsValidQuery : ApiRequest<GetPaymentCardIsValidQuery>
    {
        public Object PaymentCardId { get; set; }
    }
}
