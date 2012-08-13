using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages.BatchFirstResponseFailMessage </summary>
    [XmlRoot("BatchFirstResponseFailMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.HSBC.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BatchFirstResponseFailMessage : MsmqMessage<BatchFirstResponseFailMessage>
    {
        public String BatchReference { get; set; }
        public String ErrorCode { get; set; }
        public String FileName { get; set; }
        public String RawContents { get; set; }
    }
}
