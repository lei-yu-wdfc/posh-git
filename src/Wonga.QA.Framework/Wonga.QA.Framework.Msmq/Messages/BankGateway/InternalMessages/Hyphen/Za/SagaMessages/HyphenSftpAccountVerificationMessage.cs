using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Hyphen.Za.SagaMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages.HyphenSftpAccountVerificationMessage </summary>
    [XmlRoot("HyphenSftpAccountVerificationMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class HyphenSftpAccountVerificationMessage : MsmqMessage<HyphenSftpAccountVerificationMessage>
    {
        public Int32 BankAccountVerificationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
