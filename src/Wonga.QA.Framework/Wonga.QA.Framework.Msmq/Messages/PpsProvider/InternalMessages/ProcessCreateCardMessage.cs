using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.ProcessCreateCardMessage </summary>
    [XmlRoot("ProcessCreateCardMessage", Namespace = "Wonga.PpsProvider.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class ProcessCreateCardMessage : MsmqMessage<ProcessCreateCardMessage>
    {
        public Guid SagaId { get; set; }
    }
}
