using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetPaymentCards </summary>
    [XmlRoot("GetPaymentCards")]
    public partial class GetPaymentCardsQuery : ApiRequest<GetPaymentCardsQuery>
    {
        public Object AccountId { get; set; }
    }
}
