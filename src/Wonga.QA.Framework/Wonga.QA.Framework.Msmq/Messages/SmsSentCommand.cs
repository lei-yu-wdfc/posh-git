using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Sms.SagaMessages.SmsSentMessage </summary>
    [XmlRoot("SmsSentMessage", Namespace = "Wonga.Comms.InternalMessages.Sms.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SmsSentCommand : MsmqMessage<SmsSentCommand>
    {
        public Guid SagaId { get; set; }
    }
}
