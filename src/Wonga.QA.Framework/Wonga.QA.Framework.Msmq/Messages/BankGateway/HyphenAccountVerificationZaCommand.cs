using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("HyphenAccountVerificationMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class HyphenAccountVerificationZaCommand : MsmqMessage<HyphenAccountVerificationZaCommand>
    {
        public Guid BatchQueueId { get; set; }
        public Int32 BankAccountVerificationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
