using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("INotifyBeforeEndLoan", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public class INotifyBeforeEndLoanEvent : MsmqMessage<INotifyBeforeEndLoanEvent>
    {
        public Guid AccountId { get; set; }
        public DateTime RemindDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
