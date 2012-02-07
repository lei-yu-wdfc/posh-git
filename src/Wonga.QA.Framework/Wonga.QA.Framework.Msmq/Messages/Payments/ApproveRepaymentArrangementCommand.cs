using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("ApproveRepaymentArrangement", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ApproveRepaymentArrangementCommand : MsmqMessage<ApproveRepaymentArrangementCommand>
    {
        public Int32 RepaymentArrangementId { get; set; }
    }
}
