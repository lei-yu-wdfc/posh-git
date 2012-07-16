using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.RetrieveHolderDataFromDBMessage </summary>
    [XmlRoot("RetrieveHolderDataFromDBMessage", Namespace = "Wonga.PpsProvider.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class RetrieveHolderDataFromDBMessage : MsmqMessage<RetrieveHolderDataFromDBMessage>
    {
        public Guid SagaId { get; set; }
    }
}
