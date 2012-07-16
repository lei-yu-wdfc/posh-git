using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Hyphen.Za
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.UnMatchedFileResponseRecordMessage </summary>
    [XmlRoot("UnMatchedFileResponseRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za", DataType = "")]
    public partial class UnMatchedFileResponseRecordMessage : MsmqMessage<UnMatchedFileResponseRecordMessage>
    {
        public Object AcknowledgeRecord { get; set; }
    }
}
