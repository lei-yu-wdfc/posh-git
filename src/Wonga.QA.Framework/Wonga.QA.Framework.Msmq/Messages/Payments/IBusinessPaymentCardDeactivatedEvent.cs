using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    /// <summary> Wonga.Payments.PublicMessages.IBusinessPaymentCardDeactivated </summary>
    [XmlRoot("IBusinessPaymentCardDeactivated", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IBusinessPaymentCardDeactivatedEvent : MsmqMessage<IBusinessPaymentCardDeactivatedEvent>
    {
        public Guid PaymentCardId { get; set; }
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
