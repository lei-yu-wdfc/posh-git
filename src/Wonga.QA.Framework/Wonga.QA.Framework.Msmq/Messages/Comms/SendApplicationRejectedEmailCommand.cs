using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendApplicationRejectedEmailMessage </summary>
    [XmlRoot("SendApplicationRejectedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public partial class SendApplicationRejectedEmailCommand : MsmqMessage<SendApplicationRejectedEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
