using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.UIdInitialRequestMessage </summary>
    [XmlRoot("UIdInitialRequestMessage", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class UIdInitialRequestCommand : MsmqMessage<UIdInitialRequestCommand>
    {
        public Int32 RiskApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
