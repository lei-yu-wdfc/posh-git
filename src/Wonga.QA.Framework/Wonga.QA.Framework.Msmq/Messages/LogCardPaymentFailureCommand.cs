using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.LogCardPaymentFailureMessage </summary>
    [XmlRoot("LogCardPaymentFailureMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class LogCardPaymentFailureCommand : MsmqMessage<LogCardPaymentFailureCommand>
    {
        public DateTime FailedOn { get; set; }
        public CardPaymentStatusCodeEnum FailureCode { get; set; }
        public String FailureDescription { get; set; }
        public Guid LogEntryExternalId { get; set; }
        public Int32 ApplicationId { get; set; }
        public Int32 PaymentCardId { get; set; }
    }
}
