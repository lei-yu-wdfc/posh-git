using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IEmailUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public class IEmailUpdatedEvent : MsmqMessage<IEmailUpdatedEvent>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
