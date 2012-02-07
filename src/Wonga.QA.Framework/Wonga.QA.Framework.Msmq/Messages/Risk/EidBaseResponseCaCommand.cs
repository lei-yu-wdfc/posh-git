using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("EidBaseResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Equifax", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class EidBaseResponseCaCommand : MsmqMessage<EidBaseResponseCaCommand>
    {
        public String TransactionKey { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
