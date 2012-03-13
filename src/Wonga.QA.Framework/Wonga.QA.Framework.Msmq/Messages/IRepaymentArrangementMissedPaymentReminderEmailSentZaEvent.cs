using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Email.PublicMessages.Za.IRepaymentArrangementMissedPaymentReminderEmailSent </summary>
    [XmlRoot("IRepaymentArrangementMissedPaymentReminderEmailSent", Namespace = "Wonga.Email.PublicMessages.Za", DataType = "Wonga.Email.PublicMessages.Za.IEmailSent")]
    public partial class IRepaymentArrangementMissedPaymentReminderEmailSentZaEvent : MsmqMessage<IRepaymentArrangementMissedPaymentReminderEmailSentZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
