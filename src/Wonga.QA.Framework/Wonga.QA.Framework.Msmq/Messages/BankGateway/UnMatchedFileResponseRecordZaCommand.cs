using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.UnMatchedFileResponseRecordMessage </summary>
    [XmlRoot("UnMatchedFileResponseRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za", DataType = "")]
    public partial class UnMatchedFileResponseRecordZaCommand : MsmqMessage<UnMatchedFileResponseRecordZaCommand>
    {
        public Object AcknowledgeRecord { get; set; }
    }
}
