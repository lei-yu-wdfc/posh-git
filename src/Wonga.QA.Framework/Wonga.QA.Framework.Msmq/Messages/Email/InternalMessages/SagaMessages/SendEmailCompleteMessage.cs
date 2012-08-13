using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Email.InternalMessages.SagaMessages.SendEmailCompleteMessage </summary>
    [XmlRoot("SendEmailCompleteMessage", Namespace = "Wonga.Email.InternalMessages.SagaMessages", DataType = "Wonga.Email.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Email.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendEmailCompleteMessage : MsmqMessage<SendEmailCompleteMessage>
    {
        public Guid SagaId { get; set; }
    }
}
