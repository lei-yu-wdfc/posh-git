using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Hyphen.Za.SagaMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages.HyphenCashInMessage </summary>
    [XmlRoot("HyphenCashInMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.BankGateway.Core.Interfaces.ISendPaymentToBatchMessage" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Hyphen.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class HyphenCashInMessage : MsmqMessage<HyphenCashInMessage>
    {
        public Int32 TransactionId { get; set; }
        public Guid BatchQueueId { get; set; }
        public Guid SagaId { get; set; }
    }
}
