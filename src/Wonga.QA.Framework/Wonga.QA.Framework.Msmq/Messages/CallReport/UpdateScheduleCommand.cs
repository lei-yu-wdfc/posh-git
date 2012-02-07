using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.CallReport
{
    [XmlRoot("UpdateScheduleMessage", Namespace = "Wonga.CallReport.Batch.Handlers.InternalMessages", DataType = "")]
    public partial class UpdateScheduleCommand : MsmqMessage<UpdateScheduleCommand>
    {
        public String ScheduleName { get; set; }
    }
}
