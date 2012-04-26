using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Accounting.InternalMessages.StartScheduleMessage </summary>
    [XmlRoot("StartScheduleMessage", Namespace = "Wonga.Payments.Accounting.InternalMessages", DataType = "")]
    public partial class StartScheduleCommand : MsmqMessage<StartScheduleCommand>
    {
        public String ScheduleName { get; set; }
    }
}
