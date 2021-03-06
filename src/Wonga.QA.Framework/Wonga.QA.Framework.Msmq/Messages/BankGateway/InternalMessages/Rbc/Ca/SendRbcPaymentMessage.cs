using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Rbc.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Rbc.Ca.SendRbcPaymentMessage </summary>
    [XmlRoot("SendRbcPaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages.Rbc.Ca", DataType = "Wonga.BankGateway.InternalMessages.Ca.BasePaymentMessage,Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Rbc.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendRbcPaymentMessage : MsmqMessage<SendRbcPaymentMessage>
    {
        public Int32 TransactionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
