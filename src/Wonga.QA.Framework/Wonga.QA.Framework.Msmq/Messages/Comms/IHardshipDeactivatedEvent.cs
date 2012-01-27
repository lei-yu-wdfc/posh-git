using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IHardshipDeactivated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public class IHardshipDeactivatedEvent : MsmqMessage<IHardshipDeactivatedEvent>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
