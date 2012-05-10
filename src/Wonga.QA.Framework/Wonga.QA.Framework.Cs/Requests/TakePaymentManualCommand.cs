using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.TakePaymentManual </summary>
    [XmlRoot("TakePaymentManual")]
    public partial class TakePaymentManualCommand : CsRequest<TakePaymentManualCommand>
    {
        public Object PaymentId { get; set; }
        public Object ApplicationId { get; set; }
        public Object PaymentCardId { get; set; }
        public Object Amount { get; set; }
        public Object Currency { get; set; }
        public Object SalesforceUser { get; set; }
    }
}
