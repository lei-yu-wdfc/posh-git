using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email.Ca.SagaMessages
{
    /// <summary> Wonga.Comms.InternalMessages.Email.Ca.SagaMessages.SendPaymentReminderEmailMessage </summary>
    [XmlRoot("SendPaymentReminderEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email.Ca.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendPaymentReminderEmailCaCommand : MsmqMessage<SendPaymentReminderEmailCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String CustomerEmail { get; set; }
        public String CustomerForename { get; set; }
        public Guid SagaId { get; set; }
    }
}
