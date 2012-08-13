using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages.SagaMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.SagaMessages.StoreTextFileResponseMessage </summary>
    [XmlRoot("StoreTextFileResponseMessage", Namespace = "Wonga.FileStorage.InternalMessages.SagaMessages", DataType = "Wonga.FileStorage.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.FileStorage.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class StoreTextFileResponseMessage : MsmqMessage<StoreTextFileResponseMessage>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
