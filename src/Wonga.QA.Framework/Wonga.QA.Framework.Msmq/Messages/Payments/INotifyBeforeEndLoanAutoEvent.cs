using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("INotifyBeforeEndLoanAuto", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.INotifyBeforeEndLoan,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public class INotifyBeforeEndLoanAutoEvent : MsmqMessage<INotifyBeforeEndLoanAutoEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime RemindDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
