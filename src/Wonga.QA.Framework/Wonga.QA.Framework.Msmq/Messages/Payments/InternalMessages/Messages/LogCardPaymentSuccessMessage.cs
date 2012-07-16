using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.LogCardPaymentSuccessMessage </summary>
    [XmlRoot("LogCardPaymentSuccessMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class LogCardPaymentSuccessMessage : MsmqMessage<LogCardPaymentSuccessMessage>
    {
        public DateTime SucceededOn { get; set; }
        public Guid LogEntryExternalId { get; set; }
        public Int32 ApplicationId { get; set; }
        public Int32 PaymentCardId { get; set; }
    }
}
