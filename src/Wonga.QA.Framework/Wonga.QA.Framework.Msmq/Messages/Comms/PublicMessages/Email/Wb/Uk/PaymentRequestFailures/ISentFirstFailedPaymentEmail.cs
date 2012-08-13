using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Email.Wb.Uk.PaymentRequestFailures
{
    /// <summary> Wonga.Comms.PublicMessages.Email.Wb.Uk.PaymentRequestFailures.ISentFirstFailedPaymentEmail </summary>
    [XmlRoot("ISentFirstFailedPaymentEmail", Namespace = "Wonga.Comms.PublicMessages.Email.Wb.Uk.PaymentRequestFailures", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages.Email.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ISentFirstFailedPaymentEmail : MsmqMessage<ISentFirstFailedPaymentEmail>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
