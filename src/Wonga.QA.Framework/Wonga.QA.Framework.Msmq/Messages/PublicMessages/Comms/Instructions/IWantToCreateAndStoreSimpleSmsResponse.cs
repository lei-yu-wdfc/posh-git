using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreSimpleSmsResponse </summary>
    [XmlRoot("IWantToCreateAndStoreSimpleSmsResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToCreateAndStoreSimpleSmsResponse : MsmqMessage<IWantToCreateAndStoreSimpleSmsResponse>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
