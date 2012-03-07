using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.SubmitCounterOfferMessage </summary>
    [XmlRoot("SubmitCounterOfferMessage", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class SubmitCounterOfferRiskCommand : MsmqMessage<SubmitCounterOfferRiskCommand>
    {
        public Decimal NewLoanAmount { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
