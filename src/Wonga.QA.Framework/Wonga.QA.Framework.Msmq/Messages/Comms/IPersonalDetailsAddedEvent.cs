using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IPersonalDetailsAdded", Namespace = "Wonga.Comms.PublicMessages", DataType = "Wonga.Comms.PublicMessages.ICommsEvent")]
    public class IPersonalDetailsAddedEvent : MsmqMessage<IPersonalDetailsAddedEvent>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
