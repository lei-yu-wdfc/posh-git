using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.ExperianBulk.InternalMessages.UpdateScheduleMessage </summary>
    [XmlRoot("UpdateScheduleMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "")]
    public partial class UpdateScheduleExperianBulkCommand : MsmqMessage<UpdateScheduleExperianBulkCommand>
    {
        public String ScheduleName { get; set; }
        public List<DateTime> ReferenceDates { get; set; }
    }
}
