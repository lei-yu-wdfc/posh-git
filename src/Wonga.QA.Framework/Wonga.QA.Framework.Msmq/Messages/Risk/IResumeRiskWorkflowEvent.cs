using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IResumeRiskWorkflow </summary>
    [XmlRoot("IResumeRiskWorkflow", Namespace = "Wonga.Risk", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IResumeRiskWorkflowEvent : MsmqMessage<IResumeRiskWorkflowEvent>
    {
        public Guid SagaId { get; set; }
    }
}
