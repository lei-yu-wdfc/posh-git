using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IWorkPhoneUpdated", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "")]
    public class IWorkPhoneUpdatedZaEvent : MsmqMessage<IWorkPhoneUpdatedZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
