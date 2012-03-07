using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages.FasterThirdResponseFailureMessage </summary>
    [XmlRoot("FasterThirdResponseFailureMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "")]
    public partial class FasterThirdResponseFailureUkCommand : MsmqMessage<FasterThirdResponseFailureUkCommand>
    {
        public String FileName { get; set; }
        public String TransactionReference { get; set; }
        public String RawContents { get; set; }
        public String ErrorCode { get; set; }
    }
}
