using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendRecoverPasswordEmailMessage </summary>
    [XmlRoot("SendRecoverPasswordEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendRecoverPasswordEmailCommand : MsmqMessage<SendRecoverPasswordEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public String Url { get; set; }
        public Guid SagaId { get; set; }
    }
}
