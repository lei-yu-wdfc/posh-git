using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendAcceptedEmailMessage </summary>
    [XmlRoot("SendAcceptedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public partial class SendAcceptedEmailCommand : MsmqMessage<SendAcceptedEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
