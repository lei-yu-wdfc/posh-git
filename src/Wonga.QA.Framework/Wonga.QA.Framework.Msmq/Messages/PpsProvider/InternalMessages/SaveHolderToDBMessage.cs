using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SaveHolderToDBMessage </summary>
    [XmlRoot("SaveHolderToDBMessage", Namespace = "Wonga.PpsProvider.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class SaveHolderToDBMessage : MsmqMessage<SaveHolderToDBMessage>
    {
        public Guid SagaId { get; set; }
    }
}
