using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Scheduler.PublicMessages
{
    /// <summary> Wonga.Scheduler.PublicMessages.DailyScheduledMessage </summary>
    [XmlRoot("DailyScheduledMessage", Namespace = "Wonga.Scheduler.PublicMessages", DataType = "")]
    public partial class DailyScheduledCommand : MsmqMessage<DailyScheduledCommand>
    {
        public DateTime ReferenceTime { get; set; }
    }
}
