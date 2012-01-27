using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("IPaymentCardDeactivated", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public class IPaymentCardDeactivatedEvent : MsmqMessage<IPaymentCardDeactivatedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
