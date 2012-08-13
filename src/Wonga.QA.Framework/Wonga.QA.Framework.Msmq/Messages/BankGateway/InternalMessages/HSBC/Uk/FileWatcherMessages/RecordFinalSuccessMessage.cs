using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages.RecordFinalSuccessMessage </summary>
    [XmlRoot("RecordFinalSuccessMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.HSBC.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RecordFinalSuccessMessage : MsmqMessage<RecordFinalSuccessMessage>
    {
        public Int32 TransactionId { get; set; }
    }
}
