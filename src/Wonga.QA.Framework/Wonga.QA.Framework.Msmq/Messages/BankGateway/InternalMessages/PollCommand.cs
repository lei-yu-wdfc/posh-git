using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.PollMessage </summary>
    [XmlRoot("PollMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class PollCommand : MsmqMessage<PollCommand>
    {
        public String ScheduleName { get; set; }
        public Boolean ForcePollEvenIfInactive { get; set; }
    }
}
