using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IHardshipActivated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public class IHardshipActivatedEvent : MsmqMessage<IHardshipActivatedEvent>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
