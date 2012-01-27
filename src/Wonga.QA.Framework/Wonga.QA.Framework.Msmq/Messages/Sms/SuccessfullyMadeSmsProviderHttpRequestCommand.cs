using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Sms
{
    [XmlRoot("SuccessfullyMadeSmsProviderHttpRequestMessage", Namespace = "Wonga.Sms.InternalMessages.SagaMessages.HttpRequests", DataType = "Wonga.Sms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public class SuccessfullyMadeSmsProviderHttpRequestCommand : MsmqMessage<SuccessfullyMadeSmsProviderHttpRequestCommand>
    {
        public String ProviderMessageId { get; set; }
        public Guid SagaId { get; set; }
    }
}
