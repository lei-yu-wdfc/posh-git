using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Comms.InternalMessages.SagaMessages.FileStorageUpdateCompleteMessage </summary>
    [XmlRoot("FileStorageUpdateCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Comms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class FileStorageUpdateCompleteMessage : MsmqMessage<FileStorageUpdateCompleteMessage>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
