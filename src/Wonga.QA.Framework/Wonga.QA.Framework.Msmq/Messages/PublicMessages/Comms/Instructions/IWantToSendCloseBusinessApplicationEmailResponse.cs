using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendCloseBusinessApplicationEmailResponse </summary>
    [XmlRoot("IWantToSendCloseBusinessApplicationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendCloseBusinessApplicationEmailResponse : MsmqMessage<IWantToSendCloseBusinessApplicationEmailResponse>
    {
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
