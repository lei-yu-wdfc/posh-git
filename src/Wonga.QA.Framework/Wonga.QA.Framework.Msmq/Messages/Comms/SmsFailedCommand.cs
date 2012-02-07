using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SmsFailedMessage", Namespace = "Wonga.Comms.InternalMessages.Sms.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SmsFailedCommand : MsmqMessage<SmsFailedCommand>
    {
        public Guid SagaId { get; set; }
    }
}
