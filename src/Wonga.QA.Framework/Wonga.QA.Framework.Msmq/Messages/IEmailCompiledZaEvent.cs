using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IEmailCompiled </summary>
    [XmlRoot("IEmailCompiled", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "")]
    public partial class IEmailCompiledZaEvent : MsmqMessage<IEmailCompiledZaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
    }
}
