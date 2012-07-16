using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Iovation;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Iovation
{
    /// <summary> Wonga.Risk.Iovation.IovationResponseMessage </summary>
    [XmlRoot("IovationResponseMessage", Namespace = "Wonga.Risk.Iovation", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class IovationResponseMessage : MsmqMessage<IovationResponseMessage>
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
