using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SendApplicationRejectedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public class SendApplicationRejectedEmailCommand : MsmqMessage<SendApplicationRejectedEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
