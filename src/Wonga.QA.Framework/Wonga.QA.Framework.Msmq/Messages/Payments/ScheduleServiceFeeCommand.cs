using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ScheduleServiceFeeMessage </summary>
    [XmlRoot("ScheduleServiceFeeMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ScheduleServiceFeeCommand : MsmqMessage<ScheduleServiceFeeCommand>
    {
        public Int32 ApplicationId { get; set; }
        public DateTime FirstServiceFeeDate { get; set; }
    }
}
