using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.FailedToCreateCardMessage </summary>
    [XmlRoot("FailedToCreateCardMessage", Namespace = "Wonga.PpsProvider.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class FailedToCreateCardCommand : MsmqMessage<FailedToCreateCardCommand>
    {
        public Guid SagaId { get; set; }
    }
}
