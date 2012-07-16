using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.IExtensionAgreementProducedInternal </summary>
    [XmlRoot("IExtensionAgreementProducedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IExtensionAgreementProduced")]
    public partial class IExtensionAgreementProducedInternal : MsmqMessage<IExtensionAgreementProducedInternal>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
        public Guid ExtensionId { get; set; }
    }
}
