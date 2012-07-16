using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email.Za
{
    /// <summary> Wonga.Comms.InternalMessages.Email.Za.SendTermsAgreedEmailMessage </summary>
    [XmlRoot("SendTermsAgreedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email.Za", DataType = "Wonga.Comms.InternalMessages.Email.SendTermsAgreedEmailMessage,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendTermsAgreedEmailMessage : MsmqMessage<SendTermsAgreedEmailMessage>
    {
        public Guid DirectDebitFileId { get; set; }
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public Guid AgreementFileId { get; set; }
        public String PageUrl { get; set; }
        public Guid SagaId { get; set; }
    }
}
