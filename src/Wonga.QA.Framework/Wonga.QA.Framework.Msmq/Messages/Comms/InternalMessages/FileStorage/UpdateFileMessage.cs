using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.FileStorage
{
    /// <summary> Wonga.Comms.InternalMessages.FileStorage.UpdateFileMessage </summary>
    [XmlRoot("UpdateFileMessage", Namespace = "Wonga.Comms.InternalMessages.FileStorage", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Comms.InternalMessages.FileStorage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdateFileMessage : MsmqMessage<UpdateFileMessage>
    {
        public Guid FileId { get; set; }
        public Byte[] Content { get; set; }
        public Guid SagaId { get; set; }
    }
}
