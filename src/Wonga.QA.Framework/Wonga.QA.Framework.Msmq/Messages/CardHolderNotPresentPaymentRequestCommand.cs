using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.CardHolderNotPresentPaymentRequestMessage </summary>
    [XmlRoot("CardHolderNotPresentPaymentRequestMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "")]
    public partial class CardHolderNotPresentPaymentRequestCommand : MsmqMessage<CardHolderNotPresentPaymentRequestCommand>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Guid RequestingSagaId { get; set; }
        public Decimal TransactionAmount { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Boolean CsUserInitiated { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
