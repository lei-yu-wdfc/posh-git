using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IHomePhoneUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public class IHomePhoneUpdatedEvent : MsmqMessage<IHomePhoneUpdatedEvent>
    {
        public Guid AccountId { get; set; }
    }
}
