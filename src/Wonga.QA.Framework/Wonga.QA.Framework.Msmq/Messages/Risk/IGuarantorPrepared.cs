using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IGuarantorPrepared </summary>
    [XmlRoot("IGuarantorPrepared", Namespace = "Wonga.Risk", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IGuarantorPrepared : MsmqMessage<IGuarantorPrepared>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
