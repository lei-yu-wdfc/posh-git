using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("IRepaymentArrangementCancelled", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public class IRepaymentArrangementCancelledEvent : MsmqMessage<IRepaymentArrangementCancelledEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
