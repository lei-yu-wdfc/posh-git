using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Email
{
    [XmlRoot("IRepaymentArrangementPaymentReminderEmailSent", Namespace = "Wonga.Email.PublicMessages.Za", DataType = "Wonga.Email.PublicMessages.Za.IEmailSent")]
    public partial class IRepaymentArrangementPaymentReminderEmailSentZaEvent : MsmqMessage<IRepaymentArrangementPaymentReminderEmailSentZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
