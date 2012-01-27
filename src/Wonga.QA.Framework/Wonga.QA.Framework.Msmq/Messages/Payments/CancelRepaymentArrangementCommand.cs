using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("CancelRepaymentArrangement", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class CancelRepaymentArrangementCommand : MsmqMessage<CancelRepaymentArrangementCommand>
    {
        public Guid RepaymentArrangementId { get; set; }
    }
}
