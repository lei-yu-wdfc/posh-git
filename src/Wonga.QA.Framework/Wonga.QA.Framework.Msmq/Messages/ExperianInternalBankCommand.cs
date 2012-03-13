using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Experian.Handlers.ExperianInternalBankMessage </summary>
    [XmlRoot("ExperianInternalBankMessage", Namespace = "Wonga.Experian.Handlers", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class ExperianInternalBankCommand : MsmqMessage<ExperianInternalBankCommand>
    {
        public Guid SagaId { get; set; }
        public Object Request { get; set; }
    }
}
