using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email.Wb.Uk.PaymentRequestFailures
{
    /// <summary> Wonga.Comms.InternalMessages.Email.Wb.Uk.PaymentRequestFailures.SendFirstPaymentFailedEmailMessage </summary>
    [XmlRoot("SendFirstPaymentFailedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email.Wb.Uk.PaymentRequestFailures", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Email.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendFirstPaymentFailedEmailMessage : MsmqMessage<SendFirstPaymentFailedEmailMessage>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid FileId { get; set; }
        public String EmailAddress { get; set; }
    }
}
