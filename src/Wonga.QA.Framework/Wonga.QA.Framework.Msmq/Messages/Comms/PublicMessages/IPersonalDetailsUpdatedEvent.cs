using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IPersonalDetailsUpdated </summary>
    [XmlRoot("IPersonalDetailsUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IPersonalDetailsUpdatedEvent : MsmqMessage<IPersonalDetailsUpdatedEvent>
    {
        public Guid AccountId { get; set; }
    }
}
