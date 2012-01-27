using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Equifax
{
    [XmlRoot("EquifaxDataRequestMessage", Namespace = "Wonga.Equifax.Handlers", DataType = "NServiceBus.Saga.ISagaMessage")]
    public class EquifaxDataRequestCommand : MsmqMessage<EquifaxDataRequestCommand>
    {
        public Object Request { get; set; }
        public Guid SagaId { get; set; }
    }
}
