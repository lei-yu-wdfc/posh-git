using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email.Uk
{
    /// <summary> Wonga.Comms.InternalMessages.Email.Uk.SendPaymentConfirmationEmailMessage </summary>
    [XmlRoot("SendPaymentConfirmationEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email.Uk", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendPaymentConfirmationEmailMessage : MsmqMessage<SendPaymentConfirmationEmailMessage>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public Guid ConfirmationFileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
