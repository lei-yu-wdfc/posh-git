using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreCloseBusinessApplicationEmailResponse </summary>
    [XmlRoot("IWantToCreateAndStoreCloseBusinessApplicationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateAndStoreCloseBusinessApplicationEmailResponse : MsmqMessage<IWantToCreateAndStoreCloseBusinessApplicationEmailResponse>
    {
        public Guid ApplicationId { get; set; }
        public Object AccountIdFileIdMappings { get; set; }
        public Guid SagaId { get; set; }
    }
}
