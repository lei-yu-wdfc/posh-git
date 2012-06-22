using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IRepaymentArrangementPaymentMissed </summary>
    [XmlRoot("IRepaymentArrangementPaymentMissed", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IRepaymentArrangementPaymentMissedEvent : MsmqMessage<IRepaymentArrangementPaymentMissedEvent>
    {
        public Boolean IsEndOfGracePeriod { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
