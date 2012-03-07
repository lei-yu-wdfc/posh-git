using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.ICurrentAddressUpdated </summary>
    [XmlRoot("ICurrentAddressUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class ICurrentAddressUpdatedEvent : MsmqMessage<ICurrentAddressUpdatedEvent>
    {
        public Guid AccountId { get; set; }
    }
}
