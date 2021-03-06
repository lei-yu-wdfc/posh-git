using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.Experian
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Experian.ExperianBusinessCardRequestMessage </summary>
    [XmlRoot("ExperianBusinessCardRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Experian", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ExperianBusinessCardRequestMessage : MsmqMessage<ExperianBusinessCardRequestMessage>
    {
        public Guid PaymentCardId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
