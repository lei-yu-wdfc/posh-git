using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages.BaseSecondResponseMessage </summary>
    [XmlRoot("BaseSecondResponseMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "")]
    public partial class BaseSecondResponseUkCommand : MsmqMessage<BaseSecondResponseUkCommand>
    {
        public String TransactionReference { get; set; }
        public String FileName { get; set; }
        public String RawContents { get; set; }
    }
}
