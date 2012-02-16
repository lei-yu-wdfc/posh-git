using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("FasterThirdResponseFailureMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "")]
    public partial class FasterThirdResponseFailureUkCommand : MsmqMessage<FasterThirdResponseFailureUkCommand>
    {
        public String FileName { get; set; }
        public String TransactionReference { get; set; }
        public String RawContents { get; set; }
        public String ErrorMsg { get; set; }
    }
}
