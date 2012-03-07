using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.InternalMessages.Equifax.EIdDecisionMessage </summary>
    [XmlRoot("EIdDecisionMessage", Namespace = "Wonga.Risk.InternalMessages.Equifax", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class EIdDecisionCaCommand : MsmqMessage<EIdDecisionCaCommand>
    {
        public String TransactionKey { get; set; }
        public String[] ReasonCodes { get; set; }
        public Int32 Score { get; set; }
        public String Decision { get; set; }
        public Object RiskComponents { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
