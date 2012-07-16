using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.PublicMessages.Za
{
    /// <summary> Wonga.Email.PublicMessages.Za.IRepaymentArrangementPaymentReminderEmailSent </summary>
    [XmlRoot("IRepaymentArrangementPaymentReminderEmailSent", Namespace = "Wonga.Email.PublicMessages.Za", DataType = "Wonga.Email.PublicMessages.Za.IEmailSent")]
    public partial class IRepaymentArrangementPaymentReminderEmailSent : MsmqMessage<IRepaymentArrangementPaymentReminderEmailSent>
    {
        public Guid AccountId { get; set; }
    }
}
