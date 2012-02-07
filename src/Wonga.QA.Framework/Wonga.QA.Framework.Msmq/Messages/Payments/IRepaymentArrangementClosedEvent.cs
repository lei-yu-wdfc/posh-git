using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("IRepaymentArrangementClosed", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IRepaymentArrangementClosedEvent : MsmqMessage<IRepaymentArrangementClosedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
        public Guid ApplicationId { get; set; }
        public Boolean IsLoanEnabled { get; set; }
    }
}
