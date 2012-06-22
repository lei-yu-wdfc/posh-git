using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IExtensionAgreementProduced </summary>
    [XmlRoot("IExtensionAgreementProduced", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IExtensionAgreementProducedEvent : MsmqMessage<IExtensionAgreementProducedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
        public Guid ExtensionId { get; set; }
    }
}