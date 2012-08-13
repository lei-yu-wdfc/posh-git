using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Bmo.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bmo.Ca.SendBmoPaymentMessage </summary>
    [XmlRoot("SendBmoPaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bmo.Ca", DataType = "Wonga.BankGateway.InternalMessages.Ca.BasePaymentMessage,Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Bmo.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendBmoPaymentMessage : MsmqMessage<SendBmoPaymentMessage>
    {
        public Int32 TransactionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
