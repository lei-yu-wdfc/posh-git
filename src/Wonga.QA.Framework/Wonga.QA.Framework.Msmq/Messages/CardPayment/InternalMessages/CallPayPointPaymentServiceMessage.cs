using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.CardPayment.InternalMessages
{
    /// <summary> Wonga.CardPayment.InternalMessages.CallPayPointPaymentServiceMessage </summary>
    [XmlRoot("CallPayPointPaymentServiceMessage", Namespace = "Wonga.CardPayment.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.CardPayment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CallPayPointPaymentServiceMessage : MsmqMessage<CallPayPointPaymentServiceMessage>
    {
        public Guid SagaId { get; set; }
        public Object PaymentDetails { get; set; }
    }
}
