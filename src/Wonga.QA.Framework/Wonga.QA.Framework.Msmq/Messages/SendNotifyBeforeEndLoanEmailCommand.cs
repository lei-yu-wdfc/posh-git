using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendNotifyBeforeEndLoanEmailMessage </summary>
    [XmlRoot("SendNotifyBeforeEndLoanEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendNotifyBeforeEndLoanEmailCommand : MsmqMessage<SendNotifyBeforeEndLoanEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public Decimal Amount { get; set; }
        public String RemindDate { get; set; }
        public Guid SagaId { get; set; }
    }
}
