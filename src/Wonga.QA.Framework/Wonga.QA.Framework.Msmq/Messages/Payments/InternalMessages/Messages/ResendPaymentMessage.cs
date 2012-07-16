using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ResendPaymentMessage </summary>
    [XmlRoot("ResendPaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ResendPaymentMessage : MsmqMessage<ResendPaymentMessage>
    {
        public Guid TransactionId { get; set; }
    }
}
