using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreCloseBusinessApplicationEmailResponse </summary>
    [XmlRoot("IWantToCreateAndStoreCloseBusinessApplicationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToCreateAndStoreCloseBusinessApplicationEmailResponseEvent : MsmqMessage<IWantToCreateAndStoreCloseBusinessApplicationEmailResponseEvent>
    {
        public Guid ApplicationId { get; set; }
        public Object AccountIdFileIdMappings { get; set; }
        public Guid SagaId { get; set; }
    }
}
