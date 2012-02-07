using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("DirectDebitRegularPaymentTakenMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "Wonga.Payments.InternalMessages.SagaMessages.BaseDirectDebitMessage,Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class DirectDebitRegularPaymentTakenCommand : MsmqMessage<DirectDebitRegularPaymentTakenCommand>
    {
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Int32 PaymentNumber { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
