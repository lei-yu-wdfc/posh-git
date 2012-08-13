using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.BankServiceAvailabiltyMessage </summary>
    [XmlRoot("BankServiceAvailabiltyMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BankServiceAvailabiltyMessage : MsmqMessage<BankServiceAvailabiltyMessage>
    {
        public Int32 TransactionId { get; set; }
        public Int32 DirectDebitId { get; set; }
    }
}
