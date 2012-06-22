using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.DocumentGeneration.InternalMessages.Za
{
    /// <summary> Wonga.Comms.DocumentGeneration.InternalMessages.Za.SaveHtmlEmailFileCompleteMessage </summary>
    [XmlRoot("SaveHtmlEmailFileCompleteMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Za", DataType = "Wonga.Comms.DocumentGeneration.InternalMessages.Za.BaseSaveEmailFileCompleteMessage,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SaveHtmlEmailFileCompleteZaCommand : MsmqMessage<SaveHtmlEmailFileCompleteZaCommand>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
