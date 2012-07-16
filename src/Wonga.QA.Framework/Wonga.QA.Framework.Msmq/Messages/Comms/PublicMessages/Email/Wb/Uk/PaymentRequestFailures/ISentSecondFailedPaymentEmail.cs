using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Email.Wb.Uk.PaymentRequestFailures
{
    /// <summary> Wonga.Comms.PublicMessages.Email.Wb.Uk.PaymentRequestFailures.ISentSecondFailedPaymentEmail </summary>
    [XmlRoot("ISentSecondFailedPaymentEmail", Namespace = "Wonga.Comms.PublicMessages.Email.Wb.Uk.PaymentRequestFailures", DataType = "")]
    public partial class ISentSecondFailedPaymentEmail : MsmqMessage<ISentSecondFailedPaymentEmail>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
