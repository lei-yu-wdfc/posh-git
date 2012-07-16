using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendPasswordChangedEmailMessage </summary>
    [XmlRoot("SendPasswordChangedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public partial class SendPasswordChangedEmailMessage : MsmqMessage<SendPasswordChangedEmailMessage>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
