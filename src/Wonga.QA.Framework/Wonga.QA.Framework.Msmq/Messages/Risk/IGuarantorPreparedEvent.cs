using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.IGuarantorPrepared </summary>
    [XmlRoot("IGuarantorPrepared", Namespace = "Wonga.Risk", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IGuarantorPreparedEvent : MsmqMessage<IGuarantorPreparedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
