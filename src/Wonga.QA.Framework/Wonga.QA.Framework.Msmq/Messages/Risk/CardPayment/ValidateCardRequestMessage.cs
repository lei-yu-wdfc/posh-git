using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.CardPayment
{
    /// <summary> Wonga.Risk.CardPayment.ValidateCardRequestMessage </summary>
    [XmlRoot("ValidateCardRequestMessage", Namespace = "Wonga.Risk.CardPayment", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ValidateCardRequestMessage : MsmqMessage<ValidateCardRequestMessage>
    {
        public Guid PaymentCardID { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
