using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.SagaMessages.EmailSendCompleteMessage </summary>
    [XmlRoot("EmailSendCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class EmailSendCompleteCommand : MsmqMessage<EmailSendCompleteCommand>
    {
        public Guid SagaId { get; set; }
    }
}
