using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("IBusinessScheduledPaymentFailed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IBusinessScheduledPaymentFailedEvent : MsmqMessage<IBusinessScheduledPaymentFailedEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
