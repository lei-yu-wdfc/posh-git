using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PayU.InternalMessages
{
    /// <summary> Wonga.PayU.InternalMessages.VerifyPayUTransactionMessage </summary>
    [XmlRoot("VerifyPayUTransactionMessage", Namespace = "Wonga.PayU.InternalMessages", DataType = "")]
    public partial class VerifyPayUTransactionMessage : MsmqMessage<VerifyPayUTransactionMessage>
    {
        public Guid SafeKey { get; set; }
        public String PaymentReferenceNumber { get; set; }
    }
}
