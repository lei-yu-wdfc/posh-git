using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IRepaymentArrangementThankYouDoNotRelendEmailCompiled </summary>
    [XmlRoot("IRepaymentArrangementThankYouDoNotRelendEmailCompiled", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "Wonga.Comms.PublicMessages.Za.IEmailCompiled")]
    public partial class IRepaymentArrangementThankYouDoNotRelendEmailCompiledZaEvent : MsmqMessage<IRepaymentArrangementThankYouDoNotRelendEmailCompiledZaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
    }
}
