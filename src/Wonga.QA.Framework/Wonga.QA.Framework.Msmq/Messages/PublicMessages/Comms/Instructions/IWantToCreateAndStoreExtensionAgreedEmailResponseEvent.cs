using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreExtensionAgreedEmailResponse </summary>
    [XmlRoot("IWantToCreateAndStoreExtensionAgreedEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToCreateAndStoreExtensionAgreedEmailResponseEvent : MsmqMessage<IWantToCreateAndStoreExtensionAgreedEmailResponseEvent>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
