using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendPaymentConfirmationEmailMessage </summary>
    [XmlRoot("SendPaymentConfirmationEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Email, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendPaymentConfirmationEmailMessage : MsmqMessage<SendPaymentConfirmationEmailMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public String CustomerReference { get; set; }
        public Guid SagaId { get; set; }
    }
}
