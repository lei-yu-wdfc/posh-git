using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IRepaymentOverdue </summary>
    [XmlRoot("IRepaymentOverdue", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IRepaymentOverdueEvent : MsmqMessage<IRepaymentOverdueEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
        public Guid RepaymentArrangementDetailId { get; set; }
        public Int32 DaysOverdue { get; set; }
    }
}
