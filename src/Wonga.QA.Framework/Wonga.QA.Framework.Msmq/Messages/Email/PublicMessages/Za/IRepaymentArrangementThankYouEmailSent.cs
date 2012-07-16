using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.PublicMessages.Za
{
    /// <summary> Wonga.Email.PublicMessages.Za.IRepaymentArrangementThankYouEmailSent </summary>
    [XmlRoot("IRepaymentArrangementThankYouEmailSent", Namespace = "Wonga.Email.PublicMessages.Za", DataType = "Wonga.Email.PublicMessages.Za.IEmailSent")]
    public partial class IRepaymentArrangementThankYouEmailSent : MsmqMessage<IRepaymentArrangementThankYouEmailSent>
    {
        public Guid AccountId { get; set; }
    }
}
