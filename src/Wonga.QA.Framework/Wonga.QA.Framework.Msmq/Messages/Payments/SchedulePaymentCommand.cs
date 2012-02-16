using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("SchedulePaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class SchedulePaymentCommand : MsmqMessage<SchedulePaymentCommand>
    {
        public Guid ApplicationGuid { get; set; }
        public Int32 ApplicationId { get; set; }
        public DateTime? CollectDate { get; set; }
        public Decimal? CollectAmount { get; set; }
        public Int32? TrackingDays { get; set; }
        public Boolean IsRetry { get; set; }
        public Guid TriggerBySagaId { get; set; }
    }
}
