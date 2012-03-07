using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Experian.ExperianBureauBatchDataRequestMessage </summary>
    [XmlRoot("ExperianBureauBatchDataRequestMessage", Namespace = "Wonga.Risk.Experian", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class ExperianBureauBatchDataRequestUkCommand : MsmqMessage<ExperianBureauBatchDataRequestUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
