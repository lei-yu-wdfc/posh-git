using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    /// <summary> Wonga.BankGateway.InternalMessages.Scotiabank.Ca.TakeScotiaPaymentMessage </summary>
    [XmlRoot("TakeScotiaPaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages.Scotiabank.Ca", DataType = "Wonga.BankGateway.InternalMessages.Scotiabank.Ca.BasePaymentMessage,Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class TakeScotiaPaymentCaCommand : MsmqMessage<TakeScotiaPaymentCaCommand>
    {
        public Int32 TransactionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
