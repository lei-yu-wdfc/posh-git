using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ProcessScheduledRepaymentMessage </summary>
    [XmlRoot("ProcessScheduledRepaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ProcessScheduledRepaymentMessage : MsmqMessage<ProcessScheduledRepaymentMessage>
    {
        public Int32 RepaymentArrangementId { get; set; }
        public Int32 RepaymentDetailId { get; set; }
    }
}
