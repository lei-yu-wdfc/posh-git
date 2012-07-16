using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email.SagaMessages
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SagaMessages.SendVerificationEmailSagaMessage </summary>
    [XmlRoot("SendVerificationEmailSagaMessage", Namespace = "Wonga.Comms.InternalMessages.Email.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendVerificationEmailSagaMessage : MsmqMessage<SendVerificationEmailSagaMessage>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String UriFragment { get; set; }
        public String Forename { get; set; }
        public Guid SagaId { get; set; }
    }
}
