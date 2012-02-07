using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IDirectDebitProduced", Namespace = "Wonga.Comms.PublicMessages.Ca", DataType = "")]
    public partial class IDirectDebitProducedCaEvent : MsmqMessage<IDirectDebitProducedCaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
