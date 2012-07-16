using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.PollTimeOfDayMessage </summary>
    [XmlRoot("PollTimeOfDayMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class PollTimeOfDayMessage : MsmqMessage<PollTimeOfDayMessage>
    {
        public String ScheduleName { get; set; }
        public Boolean ForcePollEvenIfInactive { get; set; }
    }
}
