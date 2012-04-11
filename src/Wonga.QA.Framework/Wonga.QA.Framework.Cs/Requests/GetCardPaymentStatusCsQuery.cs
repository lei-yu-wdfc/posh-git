using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetCardPaymentStatusCs </summary>
    [XmlRoot("GetCardPaymentStatusCs")]
    public partial class GetCardPaymentStatusCsQuery : CsRequest<GetCardPaymentStatusCsQuery>
    {
        public Object PaymentId { get; set; }
    }
}
