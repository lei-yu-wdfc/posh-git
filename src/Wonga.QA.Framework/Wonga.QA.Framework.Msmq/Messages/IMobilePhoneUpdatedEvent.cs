using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.IMobilePhoneUpdated </summary>
    [XmlRoot("IMobilePhoneUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IMobilePhoneUpdatedEvent : MsmqMessage<IMobilePhoneUpdatedEvent>
    {
        public Guid AccountId { get; set; }
    }
}
