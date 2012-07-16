using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.CardPayment.InternalMessages
{
    /// <summary> Wonga.CardPayment.InternalMessages.SetScheduleMessage </summary>
    [XmlRoot("SetScheduleMessage", Namespace = "Wonga.CardPayment.InternalMessages", DataType = "")]
    public partial class SetScheduleMessage : MsmqMessage<SetScheduleMessage>
    {
        public Guid ServiceLoginId { get; set; }
        public Guid? ScheduleId { get; set; }
        public DateTime? ScheduleDate { get; set; }
    }
}
