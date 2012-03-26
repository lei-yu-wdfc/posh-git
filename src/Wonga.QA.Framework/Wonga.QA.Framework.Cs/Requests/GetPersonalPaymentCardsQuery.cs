using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetPersonalPaymentCards </summary>
    [XmlRoot("GetPersonalPaymentCards")]
    public partial class GetPersonalPaymentCardsQuery : CsRequest<GetPersonalPaymentCardsQuery>
    {
        public Object AccountId { get; set; }
    }
}
