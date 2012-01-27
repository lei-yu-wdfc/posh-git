using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("IRepaymentArrangementCreated", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public class IRepaymentArrangementCreatedEvent : MsmqMessage<IRepaymentArrangementCreatedEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
