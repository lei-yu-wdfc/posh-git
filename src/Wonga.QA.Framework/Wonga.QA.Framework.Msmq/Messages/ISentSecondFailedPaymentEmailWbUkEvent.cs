using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Email.Wb.Uk.PaymentRequestFailures.ISentSecondFailedPaymentEmail </summary>
    [XmlRoot("ISentSecondFailedPaymentEmail", Namespace = "Wonga.Comms.PublicMessages.Email.Wb.Uk.PaymentRequestFailures", DataType = "")]
    public partial class ISentSecondFailedPaymentEmailWbUkEvent : MsmqMessage<ISentSecondFailedPaymentEmailWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
