using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendSimpleEmailResponse </summary>
    [XmlRoot("IWantToSendSimpleEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToSendSimpleEmailResponse : MsmqMessage<IWantToSendSimpleEmailResponse>
    {
        public Boolean Successful { get; set; }
        public Guid SagaId { get; set; }
    }
}
