using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("EidInteractionQueryRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Equifax", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class EidInteractionRequestCaCommand : MsmqMessage<EidInteractionRequestCaCommand>
    {
        public String TransactionKey { get; set; }
        public String InteractiveQueryId { get; set; }
        public Object Questions { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
