using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetPaymentCardIsValid </summary>
    [XmlRoot("GetPaymentCardIsValid")]
    public partial class GetPaymentCardIsValidQuery : ApiRequest<GetPaymentCardIsValidQuery>
    {
        public Object PaymentCardId { get; set; }
    }
}
