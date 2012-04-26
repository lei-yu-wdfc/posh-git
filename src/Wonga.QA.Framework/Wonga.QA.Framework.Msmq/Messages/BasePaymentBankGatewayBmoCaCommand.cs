using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bmo.Ca.BasePaymentMessage </summary>
    [XmlRoot("BasePaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bmo.Ca", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class BasePaymentBankGatewayBmoCaCommand : MsmqMessage<BasePaymentBankGatewayBmoCaCommand>
    {
        public Int32 TransactionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
