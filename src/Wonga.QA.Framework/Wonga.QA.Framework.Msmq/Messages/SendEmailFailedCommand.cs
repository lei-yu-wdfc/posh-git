using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Email.InternalMessages.SagaMessages.SendEmailFailedMessage </summary>
    [XmlRoot("SendEmailFailedMessage", Namespace = "Wonga.Email.InternalMessages.SagaMessages", DataType = "Wonga.Email.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendEmailFailedCommand : MsmqMessage<SendEmailFailedCommand>
    {
        public Guid SagaId { get; set; }
    }
}
