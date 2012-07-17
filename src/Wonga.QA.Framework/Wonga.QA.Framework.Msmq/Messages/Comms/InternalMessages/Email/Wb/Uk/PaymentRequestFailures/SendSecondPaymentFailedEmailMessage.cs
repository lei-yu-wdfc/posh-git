using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email.Wb.Uk.PaymentRequestFailures
{
    /// <summary> Wonga.Comms.InternalMessages.Email.Wb.Uk.PaymentRequestFailures.SendSecondPaymentFailedEmailMessage </summary>
    [XmlRoot("SendSecondPaymentFailedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email.Wb.Uk.PaymentRequestFailures", DataType = "")]
    public partial class SendSecondPaymentFailedEmailMessage : MsmqMessage<SendSecondPaymentFailedEmailMessage>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid FileId { get; set; }
        public String EmailAddress { get; set; }
    }
}