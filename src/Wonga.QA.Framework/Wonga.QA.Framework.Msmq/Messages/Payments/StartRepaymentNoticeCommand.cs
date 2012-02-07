using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("StartRepaymentNoticeMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class StartRepaymentNoticeCommand : MsmqMessage<StartRepaymentNoticeCommand>
    {
        public Int32 RepaymentArrangementId { get; set; }
        public Int32 RepaymentDetailId { get; set; }
    }
}
