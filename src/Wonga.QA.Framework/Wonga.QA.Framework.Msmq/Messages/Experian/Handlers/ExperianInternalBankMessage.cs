using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Experian.Handlers
{
    /// <summary> Wonga.Experian.Handlers.ExperianInternalBankMessage </summary>
    [XmlRoot("ExperianInternalBankMessage", Namespace = "Wonga.Experian.Handlers", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class ExperianInternalBankMessage : MsmqMessage<ExperianInternalBankMessage>
    {
        public Guid SagaId { get; set; }
        public Object Request { get; set; }
    }
}
