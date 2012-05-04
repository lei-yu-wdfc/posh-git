using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bmo.Ca.PollMessage </summary>
    [XmlRoot("PollMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bmo.Ca", DataType = "")]
    public partial class PollCaCommand : MsmqMessage<PollCaCommand>
    {
        public String ScheduleName { get; set; }
        public Boolean ForcePollEvenIfInactive { get; set; }
    }
}
