using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.DocumentGeneration
{
    [XmlRoot("SavePlainEmailFileCompleteMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Za", DataType = "Wonga.Comms.DocumentGeneration.InternalMessages.Za.BaseSaveEmailFileCompleteMessage,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SavePlainEmailFileCompleteZaCommand : MsmqMessage<SavePlainEmailFileCompleteZaCommand>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
