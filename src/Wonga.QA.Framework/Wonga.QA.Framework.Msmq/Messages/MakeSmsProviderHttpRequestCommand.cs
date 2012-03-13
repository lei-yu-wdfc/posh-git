using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Sms.InternalMessages.SagaMessages.HttpRequests.MakeSmsProviderHttpRequestMessage </summary>
    [XmlRoot("MakeSmsProviderHttpRequestMessage", Namespace = "Wonga.Sms.InternalMessages.SagaMessages.HttpRequests", DataType = "Wonga.Sms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class MakeSmsProviderHttpRequestCommand : MsmqMessage<MakeSmsProviderHttpRequestCommand>
    {
        public String MobilePhoneNumber { get; set; }
        public String MessageText { get; set; }
        public Guid SagaId { get; set; }
    }
}
