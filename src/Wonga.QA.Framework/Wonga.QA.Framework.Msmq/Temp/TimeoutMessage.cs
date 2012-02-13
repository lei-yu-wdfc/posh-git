using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    [XmlRoot("TimeoutMessage", Namespace = "NServiceBus.Saga", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class TimeoutMessage : MsmqMessage<TimeoutMessage>
    {
        public DateTime Expires { get; set; }
        public Guid SagaId { get; set; }
        public Object State { get; set; }
        public Boolean ClearTimeout { get; set; }
    }
}
