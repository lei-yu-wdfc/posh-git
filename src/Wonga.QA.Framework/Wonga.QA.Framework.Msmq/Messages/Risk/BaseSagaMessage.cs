using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.BaseSagaMessage </summary>
    [XmlRoot("BaseSagaMessage", Namespace = "Wonga.Risk", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class BaseSagaMessage : MsmqMessage<BaseSagaMessage>
    {
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
