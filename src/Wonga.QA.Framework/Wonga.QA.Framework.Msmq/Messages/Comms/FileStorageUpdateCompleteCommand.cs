using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("FileStorageUpdateCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class FileStorageUpdateCompleteCommand : MsmqMessage<FileStorageUpdateCompleteCommand>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
