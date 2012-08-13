using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages.SagaMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.SagaMessages.BaseSagaMessage </summary>
    [XmlRoot("BaseSagaMessage", Namespace = "Wonga.FileStorage.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.FileStorage.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BaseSagaMessage : MsmqMessage<BaseSagaMessage>
    {
        public Guid SagaId { get; set; }
    }
}
