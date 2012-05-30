using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ConfirmIncomingPayment </summary>
    [XmlRoot("ConfirmIncomingPayment", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ConfirmIncomingPaymentCommand : MsmqMessage<ConfirmIncomingPaymentCommand>
    {
        public Int32 PaymentId { get; set; }
    }
}
