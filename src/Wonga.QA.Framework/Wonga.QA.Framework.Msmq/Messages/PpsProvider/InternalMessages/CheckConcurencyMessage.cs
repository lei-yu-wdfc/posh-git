using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.CheckConcurencyMessage </summary>
    [XmlRoot("CheckConcurencyMessage", Namespace = "Wonga.PpsProvider.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class CheckConcurencyMessage : MsmqMessage<CheckConcurencyMessage>
    {
        public Guid SagaId { get; set; }
    }
}
