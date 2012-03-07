using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SagaMessages.SendApplicationReferredEmailMessage </summary>
    [XmlRoot("SendApplicationReferredEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email.SagaMessages", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendApplicationReferredEmailCommand : MsmqMessage<SendApplicationReferredEmailCommand>
    {
        public String Email { get; set; }
        public String Forename { get; set; }
        public Guid SagaId { get; set; }
    }
}
