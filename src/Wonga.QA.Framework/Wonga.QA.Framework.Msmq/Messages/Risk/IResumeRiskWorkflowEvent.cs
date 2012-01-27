using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IResumeRiskWorkflow", Namespace = "Wonga.Risk", DataType = "NServiceBus.Saga.ISagaMessage")]
    public class IResumeRiskWorkflowEvent : MsmqMessage<IResumeRiskWorkflowEvent>
    {
        public Guid SagaId { get; set; }
    }
}
