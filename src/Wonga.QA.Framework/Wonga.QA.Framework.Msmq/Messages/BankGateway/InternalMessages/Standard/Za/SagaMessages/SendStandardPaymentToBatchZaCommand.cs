using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Standard.Za.SagaMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.Standard.Za.SagaMessages.SendStandardPaymentToBatchMessage </summary>
    [XmlRoot("SendStandardPaymentToBatchMessage", Namespace = "Wonga.BankGateway.InternalMessages.Standard.Za.SagaMessages", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.BankGateway.Core.Interfaces.ISendPaymentToBatchMessage")]
    public partial class SendStandardPaymentToBatchZaCommand : MsmqMessage<SendStandardPaymentToBatchZaCommand>
    {
        public Guid BatchQueueId { get; set; }
        public Int32 TransactionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
