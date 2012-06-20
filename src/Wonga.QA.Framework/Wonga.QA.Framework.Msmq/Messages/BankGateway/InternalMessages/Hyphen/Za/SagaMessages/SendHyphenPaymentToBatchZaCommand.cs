using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Hyphen.Za.SagaMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages.SendHyphenPaymentToBatchMessage </summary>
    [XmlRoot("SendHyphenPaymentToBatchMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za.SagaMessages", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.BankGateway.Core.Interfaces.ISendPaymentToBatchMessage")]
    public partial class SendHyphenPaymentToBatchZaCommand : MsmqMessage<SendHyphenPaymentToBatchZaCommand>
    {
        public Guid BatchQueueId { get; set; }
        public Int32 TransactionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
