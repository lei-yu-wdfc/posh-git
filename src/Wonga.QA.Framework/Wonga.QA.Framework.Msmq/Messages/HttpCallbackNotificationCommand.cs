using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Sms.InternalMessages.SagaMessages.HttpResponses.HttpCallbackNotificationMessage </summary>
    [XmlRoot("HttpCallbackNotificationMessage", Namespace = "Wonga.Sms.InternalMessages.SagaMessages.HttpResponses", DataType = "Wonga.Sms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class HttpCallbackNotificationCommand : MsmqMessage<HttpCallbackNotificationCommand>
    {
        public SmsStatusEnum Status { get; set; }
        public String ProviderStatus { get; set; }
        public Guid SagaId { get; set; }
    }
}
