using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IEmailUpdated </summary>
    [XmlRoot("IEmailUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IEmailUpdatedEvent : MsmqMessage<IEmailUpdatedEvent>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
