using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IovationRequestMessage", Namespace = "Wonga.Risk.Iovation", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public class IovationRequestCommand : MsmqMessage<IovationRequestCommand>
    {
        public Guid AccountId { get; set; }
        public String ClientIPAddress { get; set; }
        public String BlackBoxData { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
