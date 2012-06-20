using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendScheduledPingFailedEmailResponse </summary>
    [XmlRoot("IWantToSendScheduledPingFailedEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToSendScheduledPingFailedEmailResponseEvent : MsmqMessage<IWantToSendScheduledPingFailedEmailResponseEvent>
    {
        public Boolean Successful { get; set; }
        public Guid SagaId { get; set; }
    }
}
