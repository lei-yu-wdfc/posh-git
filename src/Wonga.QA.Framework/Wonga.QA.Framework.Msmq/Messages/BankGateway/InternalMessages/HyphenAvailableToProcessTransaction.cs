using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.HyphenAvailableToProcessTransaction </summary>
    [XmlRoot("HyphenAvailableToProcessTransaction", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class HyphenAvailableToProcessTransaction : MsmqMessage<HyphenAvailableToProcessTransaction>
    {
        public Int32 TransactionId { get; set; }
    }
}
