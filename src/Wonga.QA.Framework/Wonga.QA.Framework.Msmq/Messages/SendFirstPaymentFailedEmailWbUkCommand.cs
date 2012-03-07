using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Email.Wb.Uk.PaymentRequestFailures.SendFirstPaymentFailedEmailMessage </summary>
    [XmlRoot("SendFirstPaymentFailedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email.Wb.Uk.PaymentRequestFailures", DataType = "")]
    public partial class SendFirstPaymentFailedEmailWbUkCommand : MsmqMessage<SendFirstPaymentFailedEmailWbUkCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid FileId { get; set; }
        public String EmailAddress { get; set; }
    }
}
