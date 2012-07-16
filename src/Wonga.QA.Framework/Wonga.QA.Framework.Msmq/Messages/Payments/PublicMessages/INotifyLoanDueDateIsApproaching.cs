using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.INotifyLoanDueDateIsApproaching </summary>
    [XmlRoot("INotifyLoanDueDateIsApproaching", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class INotifyLoanDueDateIsApproaching : MsmqMessage<INotifyLoanDueDateIsApproaching>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Double DaysUntilDueDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
