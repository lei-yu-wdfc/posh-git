using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("ProcessScheduledPaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class ProcessScheduledPaymentCommand : MsmqMessage<ProcessScheduledPaymentCommand>
    {
        public Int32 ApplicationId { get; set; }
        public DateTime? CollectDate { get; set; }
        public Decimal? CollectAmount { get; set; }
        public Int32? TrackingDays { get; set; }
        public Boolean IsRetry { get; set; }
    }
}
