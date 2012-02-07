using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Equifax
{
    [XmlRoot("EidInitialRequestSagaMessage", Namespace = "Wonga.Equifax.Handlers", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class EidInitialRequestSagaCommand : MsmqMessage<EidInitialRequestSagaCommand>
    {
        public Guid SagaId { get; set; }
        public Object RequestData { get; set; }
    }
}
