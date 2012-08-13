using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages.FasterSecondResponseSuccessSagaMessage </summary>
    [XmlRoot("FasterSecondResponseSuccessSagaMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.HSBC.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class FasterSecondResponseSuccessSagaMessage : MsmqMessage<FasterSecondResponseSuccessSagaMessage>
    {
        public String TransactionReference { get; set; }
        public String FileName { get; set; }
        public String RawContents { get; set; }
    }
}
