using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Za
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IGenderUpdated </summary>
    [XmlRoot("IGenderUpdated", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "")]
    public partial class IGenderUpdatedZaEvent : MsmqMessage<IGenderUpdatedZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
