using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.ILoanIsDue </summary>
    [XmlRoot("ILoanIsDue", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class ILoanIsDue : MsmqMessage<ILoanIsDue>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime LoanDueDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
