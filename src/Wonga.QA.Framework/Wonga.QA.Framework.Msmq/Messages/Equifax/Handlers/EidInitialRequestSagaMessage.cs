using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Equifax.Handlers
{
    /// <summary> Wonga.Equifax.Handlers.EidInitialRequestSagaMessage </summary>
    [XmlRoot("EidInitialRequestSagaMessage", Namespace = "Wonga.Equifax.Handlers", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class EidInitialRequestSagaMessage : MsmqMessage<EidInitialRequestSagaMessage>
    {
        public Guid SagaId { get; set; }
        public Object RequestData { get; set; }
    }
}
