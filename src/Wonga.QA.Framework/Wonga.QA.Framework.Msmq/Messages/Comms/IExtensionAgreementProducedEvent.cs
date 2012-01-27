using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IExtensionAgreementProduced", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public class IExtensionAgreementProducedEvent : MsmqMessage<IExtensionAgreementProducedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
