using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.Za.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PayU.InternalMessages
{
    /// <summary> Wonga.PayU.InternalMessages.VerifyPayUTransactionResponseMessage </summary>
    [XmlRoot("VerifyPayUTransactionResponseMessage", Namespace = "Wonga.PayU.InternalMessages", DataType = "")]
    public partial class VerifyPayUTransactionResponseMessage : MsmqMessage<VerifyPayUTransactionResponseMessage>
    {
        public String PaymentReferenceNumber { get; set; }
        public PayUTransactionResultEnum Result { get; set; }
        public DateTime DateProcessed { get; set; }
        public String RawResponse { get; set; }
    }
}
