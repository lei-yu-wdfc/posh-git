using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.CallValidate
{
    [XmlRoot("CallValidateInternalMessage", Namespace = "Wonga.CallValidate.Handlers", DataType = "NServiceBus.Saga.ISagaMessage")]
    public class CallValidateInternalCommand : MsmqMessage<CallValidateInternalCommand>
    {
        public Guid SagaId { get; set; }
        public Object Request { get; set; }
    }
}
