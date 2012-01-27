using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("BaseSagaMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public class BaseSagaCommand : MsmqMessage<BaseSagaCommand>
    {
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
