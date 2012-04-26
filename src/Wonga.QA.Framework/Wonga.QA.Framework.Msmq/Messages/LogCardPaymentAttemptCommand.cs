using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.LogCardPaymentAttemptMessage </summary>
    [XmlRoot("LogCardPaymentAttemptMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class LogCardPaymentAttemptCommand : MsmqMessage<LogCardPaymentAttemptCommand>
    {
        public DateTime RequestedOn { get; set; }
        public Decimal Amount { get; set; }
        public RepaymentRequestEnum RequestType { get; set; }
        public Guid LogEntryExternalId { get; set; }
        public Int32 ApplicationId { get; set; }
        public Int32 PaymentCardId { get; set; }
    }
}
