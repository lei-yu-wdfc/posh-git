using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.CsGetCardPaymentStatus </summary>
    [XmlRoot("CsGetCardPaymentStatus")]
    public partial class CsGetCardPaymentStatusQuery : CsRequest<CsGetCardPaymentStatusQuery>
    {
        public Object PaymentId { get; set; }
    }
}
