using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Comms.InternalMessages.SagaMessages.FileStorageCompleteMessage </summary>
    [XmlRoot("FileStorageCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Comms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class FileStorageCompleteMessage : MsmqMessage<FileStorageCompleteMessage>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
