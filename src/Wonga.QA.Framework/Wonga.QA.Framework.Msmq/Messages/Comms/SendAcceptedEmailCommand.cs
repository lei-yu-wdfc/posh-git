using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SendAcceptedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public class SendAcceptedEmailCommand : MsmqMessage<SendAcceptedEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
