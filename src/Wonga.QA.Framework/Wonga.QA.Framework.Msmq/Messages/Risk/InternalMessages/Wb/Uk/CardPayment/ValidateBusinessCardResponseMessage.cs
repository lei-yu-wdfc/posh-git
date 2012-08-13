using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.CardPayment
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.CardPayment.ValidateBusinessCardResponseMessage </summary>
    [XmlRoot("ValidateBusinessCardResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.CardPayment", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ValidateBusinessCardResponseMessage : MsmqMessage<ValidateBusinessCardResponseMessage>
    {
        public Object ValidateCardResponse { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
