using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ExperianBulk
{
    [XmlRoot("BaseSagaMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class BaseSagaCommand : MsmqMessage<BaseSagaCommand>
    {
        public Guid SagaId { get; set; }
    }
}
