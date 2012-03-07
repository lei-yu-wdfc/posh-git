using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Scheduler
{
    /// <summary> Wonga.Scheduler.PublicMessages.RequestScheduleMessage </summary>
    [XmlRoot("RequestScheduleMessage", Namespace = "Wonga.Scheduler.PublicMessages", DataType = "")]
    public partial class RequestScheduleCommand : MsmqMessage<RequestScheduleCommand>
    {
        public SchedulePeriodEnum Period { get; set; }
        public DateTime ReferenceTime { get; set; }
    }
}
