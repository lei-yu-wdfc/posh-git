using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("FraudAttemptResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Salesforce", DataType = "Wonga.Risk.InternalMessages.Salesforce.ManualVerificationResponseMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class FraudAttemptResponseCommand : MsmqMessage<FraudAttemptResponseCommand>
    {
        public Int32? Probability { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
