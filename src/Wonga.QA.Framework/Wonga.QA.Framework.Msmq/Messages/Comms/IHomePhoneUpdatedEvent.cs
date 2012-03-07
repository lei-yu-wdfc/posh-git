using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.PublicMessages.IHomePhoneUpdated </summary>
    [XmlRoot("IHomePhoneUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IHomePhoneUpdatedEvent : MsmqMessage<IHomePhoneUpdatedEvent>
    {
        public Guid AccountId { get; set; }
    }
}
