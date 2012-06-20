using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Scheduler.PublicMessages;

namespace Wonga.QA.Framework.Msmq.Messages.Scheduler.PublicMessages
{
    /// <summary> Wonga.Scheduler.PublicMessages.RequestScheduleMessage </summary>
    [XmlRoot("RequestScheduleMessage", Namespace = "Wonga.Scheduler.PublicMessages", DataType = "")]
    public partial class RequestScheduleCommand : MsmqMessage<RequestScheduleCommand>
    {
        public SchedulePeriodEnum Period { get; set; }
        public DateTime ReferenceTime { get; set; }
    }
}
