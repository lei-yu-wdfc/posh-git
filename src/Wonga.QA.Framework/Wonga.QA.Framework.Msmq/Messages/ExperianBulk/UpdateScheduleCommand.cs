using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ExperianBulk
{
    [XmlRoot("UpdateScheduleMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "")]
    public class UpdateScheduleCommand : MsmqMessage<UpdateScheduleCommand>
    {
        public String ScheduleName { get; set; }
        public List<DateTime> ReferenceDates { get; set; }
    }
}
