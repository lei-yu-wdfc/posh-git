using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Iovation.IovationResponseMessage </summary>
    [XmlRoot("IovationResponseMessage", Namespace = "Wonga.Risk.Iovation", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class IovationResponseCommand : MsmqMessage<IovationResponseCommand>
    {
        public Guid AccountId { get; set; }
        public IovationAdviceEnum Result { get; set; }
        public String Reason { get; set; }
        public Object Details { get; set; }
        public String TrackingNumber { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
