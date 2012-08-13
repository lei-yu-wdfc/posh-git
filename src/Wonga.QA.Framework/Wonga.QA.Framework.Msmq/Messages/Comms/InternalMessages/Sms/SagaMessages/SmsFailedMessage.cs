using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Sms.SagaMessages
{
    /// <summary> Wonga.Comms.InternalMessages.Sms.SagaMessages.SmsFailedMessage </summary>
    [XmlRoot("SmsFailedMessage", Namespace = "Wonga.Comms.InternalMessages.Sms.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Sms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SmsFailedMessage : MsmqMessage<SmsFailedMessage>
    {
        public Guid SmsId { get; set; }
        public Guid SagaId { get; set; }
    }
}
