using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Email.Ca.SagaMessages.SendPaymentReceivedEmailMessage </summary>
    [XmlRoot("SendPaymentReceivedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email.Ca.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendPaymentReceivedEmailCaCommand : MsmqMessage<SendPaymentReceivedEmailCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TransactionId { get; set; }
        public String CustomerEmail { get; set; }
        public String CustomerForename { get; set; }
        public String CustomerSurname { get; set; }
        public String CustomerMiddleName { get; set; }
        public Guid SagaId { get; set; }
    }
}
