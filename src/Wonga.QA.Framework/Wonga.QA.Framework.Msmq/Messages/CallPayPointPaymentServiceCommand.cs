using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.CardPayment.InternalMessages.CallPayPointPaymentServiceMessage </summary>
    [XmlRoot("CallPayPointPaymentServiceMessage", Namespace = "Wonga.CardPayment.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class CallPayPointPaymentServiceCommand : MsmqMessage<CallPayPointPaymentServiceCommand>
    {
        public Guid SagaId { get; set; }
        public Object PaymentDetails { get; set; }
    }
}
