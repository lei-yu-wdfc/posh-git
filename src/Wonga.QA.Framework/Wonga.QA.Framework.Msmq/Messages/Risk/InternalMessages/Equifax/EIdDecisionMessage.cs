using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Equifax
{
    /// <summary> Wonga.Risk.InternalMessages.Equifax.EIdDecisionMessage </summary>
    [XmlRoot("EIdDecisionMessage", Namespace = "Wonga.Risk.InternalMessages.Equifax", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class EIdDecisionMessage : MsmqMessage<EIdDecisionMessage>
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
