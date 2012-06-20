using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.CallReport.Batch.Handlers.InternalMessages
{
    /// <summary> Wonga.CallReport.Batch.Handlers.InternalMessages.UpdateScheduleMessage </summary>
    [XmlRoot("UpdateScheduleMessage", Namespace = "Wonga.CallReport.Batch.Handlers.InternalMessages", DataType = "")]
    public partial class UpdateScheduleCallReportCommand : MsmqMessage<UpdateScheduleCallReportCommand>
    {
        public String ScheduleName { get; set; }
    }
}
