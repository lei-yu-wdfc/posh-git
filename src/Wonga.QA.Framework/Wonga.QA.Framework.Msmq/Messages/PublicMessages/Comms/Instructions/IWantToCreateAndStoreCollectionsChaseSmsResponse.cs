using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreCollectionsChaseSmsResponse </summary>
    [XmlRoot("IWantToCreateAndStoreCollectionsChaseSmsResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToCreateAndStoreCollectionsChaseSmsResponse : MsmqMessage<IWantToCreateAndStoreCollectionsChaseSmsResponse>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
