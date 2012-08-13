using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Scotiabank.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Scotiabank.Ca.TakeScotiaPaymentMessage </summary>
    [XmlRoot("TakeScotiaPaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages.Scotiabank.Ca", DataType = "Wonga.BankGateway.InternalMessages.Ca.BasePaymentMessage,Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Scotiabank.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class TakeScotiaPaymentMessage : MsmqMessage<TakeScotiaPaymentMessage>
    {
        public Int32 TransactionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
