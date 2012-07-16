using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendCloseBusinessApplicationEmailResponse </summary>
    [XmlRoot("IWantToSendCloseBusinessApplicationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToSendCloseBusinessApplicationEmailResponse : MsmqMessage<IWantToSendCloseBusinessApplicationEmailResponse>
    {
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
