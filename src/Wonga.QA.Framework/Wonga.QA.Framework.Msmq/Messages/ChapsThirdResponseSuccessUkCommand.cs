using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages.ChapsThirdResponseSuccessMessage </summary>
    [XmlRoot("ChapsThirdResponseSuccessMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "")]
    public partial class ChapsThirdResponseSuccessUkCommand : MsmqMessage<ChapsThirdResponseSuccessUkCommand>
    {
        public String FileName { get; set; }
        public Int32 TransactionId { get; set; }
        public String RawContents { get; set; }
    }
}
