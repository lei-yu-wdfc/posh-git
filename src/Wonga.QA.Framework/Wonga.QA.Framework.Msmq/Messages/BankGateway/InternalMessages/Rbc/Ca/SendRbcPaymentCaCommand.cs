using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Rbc.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Rbc.Ca.SendRbcPaymentMessage </summary>
    [XmlRoot("SendRbcPaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages.Rbc.Ca", DataType = "Wonga.BankGateway.InternalMessages.Ca.BasePaymentMessage,Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendRbcPaymentCaCommand : MsmqMessage<SendRbcPaymentCaCommand>
    {
        public Int32 TransactionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
