using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IRepaymentArrangementMissedPaymentReminderEmailCompiled </summary>
    [XmlRoot("IRepaymentArrangementMissedPaymentReminderEmailCompiled", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "Wonga.Comms.PublicMessages.Za.IEmailCompiled")]
    public partial class IRepaymentArrangementMissedPaymentReminderEmailCompiledZaEvent : MsmqMessage<IRepaymentArrangementMissedPaymentReminderEmailCompiledZaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
    }
}