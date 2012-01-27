using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("IRepaymentArrangementPaymentMissed", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public class IRepaymentArrangementPaymentMissedEvent : MsmqMessage<IRepaymentArrangementPaymentMissedEvent>
    {
        public Boolean IsEndOfGracePeriod { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
