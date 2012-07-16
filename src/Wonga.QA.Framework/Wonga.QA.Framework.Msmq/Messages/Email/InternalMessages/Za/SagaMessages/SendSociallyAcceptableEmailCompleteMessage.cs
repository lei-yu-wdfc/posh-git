using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages.Za.SagaMessages
{
    /// <summary> Wonga.Email.InternalMessages.Za.SagaMessages.SendSociallyAcceptableEmailCompleteMessage </summary>
    [XmlRoot("SendSociallyAcceptableEmailCompleteMessage", Namespace = "Wonga.Email.InternalMessages.Za.SagaMessages", DataType = "Wonga.Email.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendSociallyAcceptableEmailCompleteMessage : MsmqMessage<SendSociallyAcceptableEmailCompleteMessage>
    {
        public Guid SagaId { get; set; }
    }
}
