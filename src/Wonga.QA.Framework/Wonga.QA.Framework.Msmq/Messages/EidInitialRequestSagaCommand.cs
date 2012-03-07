using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Equifax.Handlers.EidInitialRequestSagaMessage </summary>
    [XmlRoot("EidInitialRequestSagaMessage", Namespace = "Wonga.Equifax.Handlers", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class EidInitialRequestSagaCommand : MsmqMessage<EidInitialRequestSagaCommand>
    {
        public Guid SagaId { get; set; }
        public Object RequestData { get; set; }
    }
}
