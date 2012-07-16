using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SaveCardToDBMessage </summary>
    [XmlRoot("SaveCardToDBMessage", Namespace = "Wonga.PpsProvider.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class SaveCardToDBMessage : MsmqMessage<SaveCardToDBMessage>
    {
        public Guid SagaId { get; set; }
    }
}
