using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages.HyphenSftpAccountVerificationMessage </summary>
    [XmlRoot("HyphenSftpAccountVerificationMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class HyphenSftpAccountVerificationZaCommand : MsmqMessage<HyphenSftpAccountVerificationZaCommand>
    {
        public Int32 BankAccountVerificationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
