using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Email
{
    /// <summary> Wonga.Email.PublicMessages.Za.IRepaymentArrangementMissedPaymentEmailSent </summary>
    [XmlRoot("IRepaymentArrangementMissedPaymentEmailSent", Namespace = "Wonga.Email.PublicMessages.Za", DataType = "Wonga.Email.PublicMessages.Za.IEmailSent")]
    public partial class IRepaymentArrangementMissedPaymentEmailSentZaEvent : MsmqMessage<IRepaymentArrangementMissedPaymentEmailSentZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
