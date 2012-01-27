using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("ICurrentAddressAdded", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public class ICurrentAddressAddedEvent : MsmqMessage<ICurrentAddressAddedEvent>
    {
        public Guid AccountId { get; set; }
    }
}
