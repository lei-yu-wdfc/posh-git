using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Transunion
{
    /// <summary> Wonga.Risk.InternalMessages.Transunion.TransunionRequestMessage </summary>
    [XmlRoot("TransunionRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Transunion", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class TransunionRequestMessage : MsmqMessage<TransunionRequestMessage>
    {
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
