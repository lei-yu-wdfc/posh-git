using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IMobilePhoneUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public class IMobilePhoneUpdatedEvent : MsmqMessage<IMobilePhoneUpdatedEvent>
    {
        public Guid AccountId { get; set; }
    }
}
