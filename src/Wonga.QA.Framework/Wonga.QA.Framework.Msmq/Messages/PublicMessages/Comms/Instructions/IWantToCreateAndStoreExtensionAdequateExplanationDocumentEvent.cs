using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreExtensionAdequateExplanationDocument </summary>
    [XmlRoot("IWantToCreateAndStoreExtensionAdequateExplanationDocument", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreExtensionAdequateExplanationDocumentEvent : MsmqMessage<IWantToCreateAndStoreExtensionAdequateExplanationDocumentEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
