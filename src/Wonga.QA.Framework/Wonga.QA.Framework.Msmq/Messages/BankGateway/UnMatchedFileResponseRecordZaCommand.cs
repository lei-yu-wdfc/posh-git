using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("UnMatchedFileResponseRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za", DataType = "")]
    public partial class UnMatchedFileResponseRecordZaCommand : MsmqMessage<UnMatchedFileResponseRecordZaCommand>
    {
        public Object AcknowledgeRecord { get; set; }
    }
}
