using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.Ca;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.CardHolderNotPresentPaymentRequestInternalMessage </summary>
    [XmlRoot("CardHolderNotPresentPaymentRequestInternalMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "")]
    public partial class CardHolderNotPresentPaymentRequestInternalCommand : MsmqMessage<CardHolderNotPresentPaymentRequestInternalCommand>
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
