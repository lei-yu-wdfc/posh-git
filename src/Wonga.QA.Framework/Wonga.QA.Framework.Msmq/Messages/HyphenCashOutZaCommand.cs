using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages.HyphenCashOutMessage </summary>
    [XmlRoot("HyphenCashOutMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.BankGateway.Core.Interfaces.ISendPaymentToBatchMessage")]
    public partial class HyphenCashOutZaCommand : MsmqMessage<HyphenCashOutZaCommand>
    {
        public Int32 TransactionId { get; set; }
        public Guid BatchQueueId { get; set; }
        public Guid SagaId { get; set; }
    }
}
