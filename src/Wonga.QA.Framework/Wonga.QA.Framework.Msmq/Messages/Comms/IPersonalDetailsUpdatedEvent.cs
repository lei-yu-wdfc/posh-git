using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IPersonalDetailsUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public class IPersonalDetailsUpdatedEvent : MsmqMessage<IPersonalDetailsUpdatedEvent>
    {
        public Guid AccountId { get; set; }
    }
}
