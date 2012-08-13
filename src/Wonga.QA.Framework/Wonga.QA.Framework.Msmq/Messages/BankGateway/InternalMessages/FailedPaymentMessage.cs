using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.FailedPaymentMessage </summary>
    [XmlRoot("FailedPaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class FailedPaymentMessage : MsmqMessage<FailedPaymentMessage>
    {
        public Int32 TransactionId { get; set; }
        public String ErrorMessage { get; set; }
    }
}
