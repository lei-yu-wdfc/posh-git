using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendNotifyBeforeEndLoanEmailMessage </summary>
    [XmlRoot("SendNotifyBeforeEndLoanEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendNotifyBeforeEndLoanEmailMessage : MsmqMessage<SendNotifyBeforeEndLoanEmailMessage>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public Guid? PaymentTransactionId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
