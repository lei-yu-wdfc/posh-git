using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IRepaymentArrangementCreatedEmailCompiled", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "Wonga.Comms.PublicMessages.Za.IEmailCompiled")]
    public partial class IRepaymentArrangementCreatedEmailCompiledZaEvent : MsmqMessage<IRepaymentArrangementCreatedEmailCompiledZaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
    }
}
