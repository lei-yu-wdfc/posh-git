using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Email.InternalMessages.SagaMessages.SendEmailCompleteMessage </summary>
    [XmlRoot("SendEmailCompleteMessage", Namespace = "Wonga.Email.InternalMessages.SagaMessages", DataType = "Wonga.Email.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendEmailCompleteCommand : MsmqMessage<SendEmailCompleteCommand>
    {
        public Guid SagaId { get; set; }
    }
}
