using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendExtensionAgreementMessage </summary>
    [XmlRoot("SendExtensionAgreementMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Email, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendExtensionAgreementMessage : MsmqMessage<SendExtensionAgreementMessage>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public Guid AgreementFileId { get; set; }
        public String PageUrl { get; set; }
        public Decimal TotalRepayable { get; set; }
        public DateTime PromiseDate { get; set; }
        public Guid SagaId { get; set; }
    }
}
