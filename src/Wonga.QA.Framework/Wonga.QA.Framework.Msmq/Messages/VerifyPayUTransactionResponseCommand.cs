using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PayU.InternalMessages.VerifyPayUTransactionResponseMessage </summary>
    [XmlRoot("VerifyPayUTransactionResponseMessage", Namespace = "Wonga.PayU.InternalMessages", DataType = "")]
    public partial class VerifyPayUTransactionResponseCommand : MsmqMessage<VerifyPayUTransactionResponseCommand>
    {
        public String PaymentReferenceNumber { get; set; }
        public PayUTransactionResultEnum Result { get; set; }
        public DateTime DateProcessed { get; set; }
        public String RawResponse { get; set; }
    }
}
