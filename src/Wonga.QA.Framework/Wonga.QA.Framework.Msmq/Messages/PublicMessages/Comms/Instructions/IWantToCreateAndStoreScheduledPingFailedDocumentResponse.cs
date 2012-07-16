using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreScheduledPingFailedDocumentResponse </summary>
    [XmlRoot("IWantToCreateAndStoreScheduledPingFailedDocumentResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToCreateAndStoreScheduledPingFailedDocumentResponse : MsmqMessage<IWantToCreateAndStoreScheduledPingFailedDocumentResponse>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
