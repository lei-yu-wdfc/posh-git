using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages.RecordFinalFailureMessage </summary>
    [XmlRoot("RecordFinalFailureMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.HSBC.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RecordFinalFailureMessage : MsmqMessage<RecordFinalFailureMessage>
    {
        public String TransactionReference { get; set; }
        public String ErrorCode { get; set; }
    }
}
