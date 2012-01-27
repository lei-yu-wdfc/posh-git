using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.CallReport
{
    [XmlRoot("UpdateScheduleMessage", Namespace = "Wonga.CallReport.InternalMessages", DataType = "")]
    public class UpdateScheduleCommand : MsmqMessage<UpdateScheduleCommand>
    {
        public String ScheduleName { get; set; }
        public List<DateTime> ReferenceDates { get; set; }
    }
}
