using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PayU.InternalMessages
{
    /// <summary> Wonga.PayU.InternalMessages.VerifyPayUTransactionMessage </summary>
    [XmlRoot("VerifyPayUTransactionMessage", Namespace = "Wonga.PayU.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.PayU.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class VerifyPayUTransactionMessage : MsmqMessage<VerifyPayUTransactionMessage>
    {
        public Guid SafeKey { get; set; }
        public String PaymentReferenceNumber { get; set; }
    }
}
