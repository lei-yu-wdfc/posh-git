using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Experian
{
    /// <summary> Wonga.Experian.Handlers.ExperianInternalCardMessage </summary>
    [XmlRoot("ExperianInternalCardMessage", Namespace = "Wonga.Experian.Handlers", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class ExperianInternalCardCommand : MsmqMessage<ExperianInternalCardCommand>
    {
        public Guid SagaId { get; set; }
        public Object Request { get; set; }
    }
}
