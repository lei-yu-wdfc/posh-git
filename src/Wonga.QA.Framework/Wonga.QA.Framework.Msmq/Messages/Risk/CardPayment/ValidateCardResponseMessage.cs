using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.CardPayment
{
    /// <summary> Wonga.Risk.CardPayment.ValidateCardResponseMessage </summary>
    [XmlRoot("ValidateCardResponseMessage", Namespace = "Wonga.Risk.CardPayment", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ValidateCardResponseMessage : MsmqMessage<ValidateCardResponseMessage>
    {
        public Object ValidateCardResponse { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
