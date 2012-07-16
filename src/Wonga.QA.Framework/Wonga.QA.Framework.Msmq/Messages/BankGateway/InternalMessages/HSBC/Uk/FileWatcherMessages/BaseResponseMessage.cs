using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages.BaseResponseMessage </summary>
    [XmlRoot("BaseResponseMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "")]
    public partial class BaseResponseMessage : MsmqMessage<BaseResponseMessage>
    {
        public String TransactionReference { get; set; }
        public String FileName { get; set; }
        public String RawContents { get; set; }
    }
}
