using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Email
{
    [XmlRoot("IRepaymentArrangementCancelledEmailSent", Namespace = "Wonga.Email.PublicMessages.Za", DataType = "Wonga.Email.PublicMessages.Za.IEmailSent")]
    public partial class IRepaymentArrangementCancelledEmailSentZaEvent : MsmqMessage<IRepaymentArrangementCancelledEmailSentZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
