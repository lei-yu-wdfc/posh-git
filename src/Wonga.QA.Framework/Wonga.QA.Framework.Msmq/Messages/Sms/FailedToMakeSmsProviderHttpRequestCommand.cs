using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Sms
{
    /// <summary> Wonga.Sms.InternalMessages.SagaMessages.HttpRequests.FailedToMakeSmsProviderHttpRequestMessage </summary>
    [XmlRoot("FailedToMakeSmsProviderHttpRequestMessage", Namespace = "Wonga.Sms.InternalMessages.SagaMessages.HttpRequests", DataType = "Wonga.Sms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class FailedToMakeSmsProviderHttpRequestCommand : MsmqMessage<FailedToMakeSmsProviderHttpRequestCommand>
    {
        public String Error { get; set; }
        public Guid SagaId { get; set; }
    }
}
