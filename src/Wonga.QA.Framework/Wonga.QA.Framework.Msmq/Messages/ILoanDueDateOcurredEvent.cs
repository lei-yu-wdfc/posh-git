using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.ILoanDueDateOcurred </summary>
    [XmlRoot("ILoanDueDateOcurred", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class ILoanDueDateOcurredEvent : MsmqMessage<ILoanDueDateOcurredEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime LoanDueDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
