using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.CardPayment
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.CardPayment.ValidateBusinessCardResponseMessage </summary>
    [XmlRoot("ValidateBusinessCardResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.CardPayment", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class ValidateBusinessCardResponseWbUkCommand : MsmqMessage<ValidateBusinessCardResponseWbUkCommand>
    {
        public Object ValidateCardResponse { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
