using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.CardPayment
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.CardPayment.ValidateBusinessCardRequestMessage </summary>
    [XmlRoot("ValidateBusinessCardRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.CardPayment", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ValidateBusinessCardRequestMessage : MsmqMessage<ValidateBusinessCardRequestMessage>
    {
        public Guid PaymentCardID { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
