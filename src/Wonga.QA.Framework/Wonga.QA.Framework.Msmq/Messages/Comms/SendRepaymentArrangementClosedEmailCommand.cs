using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SendRepaymentArrangementClosedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.Email.BaseSimpleEmailMessage,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public class SendRepaymentArrangementClosedEmailCommand : MsmqMessage<SendRepaymentArrangementClosedEmailCommand>
    {
        public String FirstName { get; set; }
        public String Email { get; set; }
        public Guid SagaId { get; set; }
    }
}
