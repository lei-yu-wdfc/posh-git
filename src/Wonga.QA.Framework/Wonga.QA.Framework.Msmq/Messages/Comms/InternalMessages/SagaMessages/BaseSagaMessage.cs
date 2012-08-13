using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage </summary>
    [XmlRoot("BaseSagaMessage", Namespace = "Wonga.Comms.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Comms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BaseSagaMessage : MsmqMessage<BaseSagaMessage>
    {
        public Guid SagaId { get; set; }
    }
}
