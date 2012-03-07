using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.InternalMessages.Transunion.TransunionRequestMessage </summary>
    [XmlRoot("TransunionRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Transunion", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class TransunionRequestZaCommand : MsmqMessage<TransunionRequestZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
