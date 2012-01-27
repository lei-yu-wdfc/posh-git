using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("ProcessScheduledRepaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class ProcessScheduledRepaymentCommand : MsmqMessage<ProcessScheduledRepaymentCommand>
    {
        public Int32 RepaymentArrangementId { get; set; }
        public Int32 RepaymentDetailId { get; set; }
    }
}
