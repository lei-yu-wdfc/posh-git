using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("TransunionResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Transunion", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public class TransunionResponseZaCommand : MsmqMessage<TransunionResponseZaCommand>
    {
        public Guid AccountId { get; set; }
        public Object Response { get; set; }
        public String InputForename { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
