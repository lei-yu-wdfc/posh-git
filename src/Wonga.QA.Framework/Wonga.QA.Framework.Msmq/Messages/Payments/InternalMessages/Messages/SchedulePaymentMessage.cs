using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.SchedulePaymentMessage </summary>
    [XmlRoot("SchedulePaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SchedulePaymentMessage : MsmqMessage<SchedulePaymentMessage>
    {
        public Guid ApplicationGuid { get; set; }
        public Int32 ApplicationId { get; set; }
        public DateTime? CollectDate { get; set; }
        public DateTime InterestOnDate { get; set; }
        public Decimal? CollectAmount { get; set; }
        public Int32? TrackingDays { get; set; }
        public Boolean IsRetry { get; set; }
        public Guid TriggerBySagaId { get; set; }
    }
}
