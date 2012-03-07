using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages.BatchFirstResponseFailMessage </summary>
    [XmlRoot("BatchFirstResponseFailMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "")]
    public partial class BatchFirstResponseFailUkCommand : MsmqMessage<BatchFirstResponseFailUkCommand>
    {
        public String BatchReference { get; set; }
        public String ErrorCode { get; set; }
        public String FileName { get; set; }
        public String RawContents { get; set; }
    }
}
