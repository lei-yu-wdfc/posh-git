using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Ca.BasePaymentMessage </summary>
    [XmlRoot("BasePaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages.Ca", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BasePaymentMessage : MsmqMessage<BasePaymentMessage>
    {
        public Int32 TransactionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
