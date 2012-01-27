using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CompleteEmailVerificationSagaMessage", Namespace = "Wonga.Comms.InternalMessages.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public class CompleteEmailVerificationSagaCommand : MsmqMessage<CompleteEmailVerificationSagaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
    }
}
