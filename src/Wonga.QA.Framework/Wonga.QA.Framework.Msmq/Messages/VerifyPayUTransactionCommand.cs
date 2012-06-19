using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PayU.InternalMessages.VerifyPayUTransactionMessage </summary>
    [XmlRoot("VerifyPayUTransactionMessage", Namespace = "Wonga.PayU.InternalMessages", DataType = "")]
    public partial class VerifyPayUTransactionCommand : MsmqMessage<VerifyPayUTransactionCommand>
    {
        public Guid SafeKey { get; set; }
        public String PaymentReferenceNumber { get; set; }
    }
}
