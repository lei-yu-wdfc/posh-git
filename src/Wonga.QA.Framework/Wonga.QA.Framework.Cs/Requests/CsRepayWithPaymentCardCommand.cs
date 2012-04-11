using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsRepayWithPaymentCard </summary>
    [XmlRoot("CsRepayWithPaymentCard")]
    public partial class CsRepayWithPaymentCardCommand : CsRequest<CsRepayWithPaymentCardCommand>
    {
        public Object AccountId { get; set; }
        public Object PaymentCardId { get; set; }
        public Object Amount { get; set; }
        public Object Currency { get; set; }
        public Object CV2 { get; set; }
        public Object CSUser { get; set; }
    }
}
