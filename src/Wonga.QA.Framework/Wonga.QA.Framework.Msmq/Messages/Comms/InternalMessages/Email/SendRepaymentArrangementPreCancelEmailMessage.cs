using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendRepaymentArrangementPreCancelEmailMessage </summary>
    [XmlRoot("SendRepaymentArrangementPreCancelEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.Email.BaseSimpleEmailMessage,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendRepaymentArrangementPreCancelEmailMessage : MsmqMessage<SendRepaymentArrangementPreCancelEmailMessage>
    {
        public String FirstName { get; set; }
        public Decimal RepaymentAmount { get; set; }
        public DateTime RepaymentDueDate { get; set; }
        public DateTime GracePeriodDeadline { get; set; }
        public String Email { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
