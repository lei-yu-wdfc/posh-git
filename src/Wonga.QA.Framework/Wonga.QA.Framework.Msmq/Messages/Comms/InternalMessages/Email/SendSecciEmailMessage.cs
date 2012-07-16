using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendSecciEmailMessage </summary>
    [XmlRoot("SendSecciEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendSecciEmailMessage : MsmqMessage<SendSecciEmailMessage>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public Guid SecciFileId { get; set; }
        public String Forename { get; set; }
        public String PageUrl { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid TopupId { get; set; }
        public Guid ApplictionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
