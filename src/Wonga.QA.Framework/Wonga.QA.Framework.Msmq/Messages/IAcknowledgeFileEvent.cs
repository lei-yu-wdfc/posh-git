using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.Core.Interfaces.IAcknowledgeFileMessage </summary>
    [XmlRoot("IAcknowledgeFileMessage", Namespace = "Wonga.BankGateway.Core.Interfaces", DataType = "")]
    public partial class IAcknowledgeFileEvent : MsmqMessage<IAcknowledgeFileEvent>
    {
        public String FileName { get; set; }
        public Byte[] FileContents { get; set; }
        public Int32 AcknowledgeTypeId { get; set; }
    }
}
