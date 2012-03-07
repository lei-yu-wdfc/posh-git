using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.ICurrentAddressAdded </summary>
    [XmlRoot("ICurrentAddressAdded", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class ICurrentAddressAddedEvent : MsmqMessage<ICurrentAddressAddedEvent>
    {
        public Guid AccountId { get; set; }
    }
}
