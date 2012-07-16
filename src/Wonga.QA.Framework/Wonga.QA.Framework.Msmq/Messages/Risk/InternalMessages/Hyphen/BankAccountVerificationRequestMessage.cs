using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Hyphen
{
    /// <summary> Wonga.Risk.InternalMessages.Hyphen.BankAccountVerificationRequestMessage </summary>
    [XmlRoot("BankAccountVerificationRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Hyphen", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class BankAccountVerificationRequestMessage : MsmqMessage<BankAccountVerificationRequestMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
