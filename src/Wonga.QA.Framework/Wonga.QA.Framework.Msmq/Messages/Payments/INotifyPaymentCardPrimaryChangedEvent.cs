using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("INotifyPaymentCardPrimaryChanged", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public class INotifyPaymentCardPrimaryChangedEvent : MsmqMessage<INotifyPaymentCardPrimaryChangedEvent>
    {
        public Guid PaymentCardId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
