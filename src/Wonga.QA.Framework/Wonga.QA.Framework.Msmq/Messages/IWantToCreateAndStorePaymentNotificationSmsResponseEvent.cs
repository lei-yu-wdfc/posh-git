using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.IWantToCreateAndStorePaymentNotificationSmsResponse </summary>
    [XmlRoot("IWantToCreateAndStorePaymentNotificationSmsResponse", Namespace = "Wonga.PublicMessages.Comms", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToCreateAndStorePaymentNotificationSmsResponseEvent : MsmqMessage<IWantToCreateAndStorePaymentNotificationSmsResponseEvent>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
