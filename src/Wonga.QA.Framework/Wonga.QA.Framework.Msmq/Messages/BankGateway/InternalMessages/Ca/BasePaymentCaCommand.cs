using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Ca.BasePaymentMessage </summary>
    [XmlRoot("BasePaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages.Ca", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class BasePaymentCaCommand : MsmqMessage<BasePaymentCaCommand>
    {
        public Int32 TransactionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
