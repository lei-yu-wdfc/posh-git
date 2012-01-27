using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IDirectDebitProduced", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "")]
    public class IDirectDebitProducedZaEvent : MsmqMessage<IDirectDebitProducedZaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
