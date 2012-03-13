using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Email.PublicMessages.Za.IRepaymentArrangementPartiallyRepaidEmailSent </summary>
    [XmlRoot("IRepaymentArrangementPartiallyRepaidEmailSent", Namespace = "Wonga.Email.PublicMessages.Za", DataType = "Wonga.Email.PublicMessages.Za.IEmailSent")]
    public partial class IRepaymentArrangementPartiallyRepaidEmailSentZaEvent : MsmqMessage<IRepaymentArrangementPartiallyRepaidEmailSentZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
