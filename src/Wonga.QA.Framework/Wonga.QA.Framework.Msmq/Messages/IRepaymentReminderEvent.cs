using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.IRepaymentReminder </summary>
    [XmlRoot("IRepaymentReminder", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IRepaymentReminderEvent : MsmqMessage<IRepaymentReminderEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
        public Guid RepaymentArrangementDetailId { get; set; }
        public Int32 DaysBefore { get; set; }
    }
}
