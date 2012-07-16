using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendTopUpAgreementEmailMessage </summary>
    [XmlRoot("SendTopUpAgreementEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendTopUpAgreementEmailMessage : MsmqMessage<SendTopUpAgreementEmailMessage>
    {
        public String Email { get; set; }
        public String Forename { get; set; }
        public Guid AgreementFileId { get; set; }
        public String PageUrl { get; set; }
        public Guid TopupId { get; set; }
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
    }
}
