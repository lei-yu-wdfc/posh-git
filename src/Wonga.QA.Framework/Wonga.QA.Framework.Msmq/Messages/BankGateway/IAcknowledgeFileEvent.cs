using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("IAcknowledgeFileMessage", Namespace = "Wonga.BankGateway.Core.Interfaces", DataType = "")]
    public class IAcknowledgeFileEvent : MsmqMessage<IAcknowledgeFileEvent>
    {
        public String FileName { get; set; }
        public Byte[] FileContents { get; set; }
        public Int32 AcknowledgeTypeId { get; set; }
    }
}
