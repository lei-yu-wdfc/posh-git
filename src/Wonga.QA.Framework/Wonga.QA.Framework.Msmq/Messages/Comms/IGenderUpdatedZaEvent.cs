using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IGenderUpdated", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "")]
    public partial class IGenderUpdatedZaEvent : MsmqMessage<IGenderUpdatedZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
