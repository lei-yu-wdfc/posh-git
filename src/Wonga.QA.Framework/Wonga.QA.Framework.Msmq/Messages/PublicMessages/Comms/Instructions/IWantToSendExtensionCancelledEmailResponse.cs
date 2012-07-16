using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendExtensionCancelledEmailResponse </summary>
    [XmlRoot("IWantToSendExtensionCancelledEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendExtensionCancelledEmailResponse : MsmqMessage<IWantToSendExtensionCancelledEmailResponse>
    {
        public Guid SagaId { get; set; }
    }
}
