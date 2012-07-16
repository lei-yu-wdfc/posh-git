using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.CardPayment
{
    /// <summary> Wonga.Risk.CardPayment.ValidateCardResponseMessage </summary>
    [XmlRoot("ValidateCardResponseMessage", Namespace = "Wonga.Risk.CardPayment", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class ValidateCardResponseMessage : MsmqMessage<ValidateCardResponseMessage>
    {
        public Object ValidateCardResponse { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
