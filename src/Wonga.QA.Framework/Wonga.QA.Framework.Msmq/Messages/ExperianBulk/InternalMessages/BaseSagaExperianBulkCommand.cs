using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.ExperianBulk.InternalMessages
{
    /// <summary> Wonga.ExperianBulk.InternalMessages.BaseSagaMessage </summary>
    [XmlRoot("BaseSagaMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class BaseSagaExperianBulkCommand : MsmqMessage<BaseSagaExperianBulkCommand>
    {
        public Guid SagaId { get; set; }
    }
}
