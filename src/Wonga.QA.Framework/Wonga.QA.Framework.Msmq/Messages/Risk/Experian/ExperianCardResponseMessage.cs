using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Experian
{
    /// <summary> Wonga.Risk.Experian.ExperianCardResponseMessage </summary>
    [XmlRoot("ExperianCardResponseMessage", Namespace = "Wonga.Risk.Experian", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class ExperianCardResponseMessage : MsmqMessage<ExperianCardResponseMessage>
    {
        public Object Response { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
