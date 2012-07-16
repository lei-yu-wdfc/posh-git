using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendExtensionAgreedEmailResponse </summary>
    [XmlRoot("IWantToSendExtensionAgreedEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendExtensionAgreedEmailResponse : MsmqMessage<IWantToSendExtensionAgreedEmailResponse>
    {
        public Guid SagaId { get; set; }
    }
}
