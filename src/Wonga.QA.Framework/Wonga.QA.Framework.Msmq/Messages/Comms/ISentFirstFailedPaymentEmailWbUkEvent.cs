using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("ISentFirstFailedPaymentEmail", Namespace = "Wonga.Comms.PublicMessages.Email.Wb.Uk.PaymentRequestFailures", DataType = "")]
    public partial class ISentFirstFailedPaymentEmailWbUkEvent : MsmqMessage<ISentFirstFailedPaymentEmailWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
