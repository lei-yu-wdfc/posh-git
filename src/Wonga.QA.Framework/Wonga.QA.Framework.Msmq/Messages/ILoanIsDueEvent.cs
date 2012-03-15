using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.ILoanIsDue </summary>
    [XmlRoot("ILoanIsDue", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class ILoanIsDueEvent : MsmqMessage<ILoanIsDueEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime LoanDueDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
