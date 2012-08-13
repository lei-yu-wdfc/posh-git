using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Sms.InternalMessages.SagaMessages.HttpRequests
{
    /// <summary> Wonga.Sms.InternalMessages.SagaMessages.HttpRequests.SuccessfullyMadeSmsProviderHttpRequestMessage </summary>
    [XmlRoot("SuccessfullyMadeSmsProviderHttpRequestMessage", Namespace = "Wonga.Sms.InternalMessages.SagaMessages.HttpRequests", DataType = "Wonga.Sms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Sms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SuccessfullyMadeSmsProviderHttpRequestMessage : MsmqMessage<SuccessfullyMadeSmsProviderHttpRequestMessage>
    {
        public String ProviderMessageId { get; set; }
        public Guid SagaId { get; set; }
    }
}
