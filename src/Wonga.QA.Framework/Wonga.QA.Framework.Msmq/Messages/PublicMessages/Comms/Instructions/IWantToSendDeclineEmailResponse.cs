using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendDeclineEmailResponse </summary>
    [XmlRoot("IWantToSendDeclineEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendDeclineEmailResponse : MsmqMessage<IWantToSendDeclineEmailResponse>
    {
        public Guid SagaId { get; set; }
    }
}
