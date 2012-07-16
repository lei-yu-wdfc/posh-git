using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Equifax
{
    /// <summary> Wonga.Risk.InternalMessages.Equifax.EidDecisionResponseMessage </summary>
    [XmlRoot("EidDecisionResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Equifax", DataType = "Wonga.Risk.InternalMessages.Equifax.EidBaseResponseMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class EidDecisionResponseMessage : MsmqMessage<EidDecisionResponseMessage>
    {
        public String[] ReasonCodes { get; set; }
        public Int32 Score { get; set; }
        public String Decision { get; set; }
        public Object RiskComponents { get; set; }
        public String TransactionKey { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
