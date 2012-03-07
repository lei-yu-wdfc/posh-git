using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.IHardshipActivated </summary>
    [XmlRoot("IHardshipActivated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IHardshipActivatedEvent : MsmqMessage<IHardshipActivatedEvent>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
