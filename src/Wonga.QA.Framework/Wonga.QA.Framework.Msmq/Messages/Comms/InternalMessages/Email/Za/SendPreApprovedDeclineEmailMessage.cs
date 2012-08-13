using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email.Za
{
    /// <summary> Wonga.Comms.InternalMessages.Email.Za.SendPreApprovedDeclineEmailMessage </summary>
    [XmlRoot("SendPreApprovedDeclineEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email.Za", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Email.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendPreApprovedDeclineEmailMessage : MsmqMessage<SendPreApprovedDeclineEmailMessage>
    {
        public Guid ApplicationId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public Guid SagaId { get; set; }
    }
}
