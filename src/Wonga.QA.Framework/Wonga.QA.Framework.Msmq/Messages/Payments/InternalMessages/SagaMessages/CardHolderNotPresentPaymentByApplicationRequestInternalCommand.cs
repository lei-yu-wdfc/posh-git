using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.CardHolderNotPresentPaymentByApplicationRequestInternalMessage </summary>
    [XmlRoot("CardHolderNotPresentPaymentByApplicationRequestInternalMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "")]
    public partial class CardHolderNotPresentPaymentByApplicationRequestInternalCommand : MsmqMessage<CardHolderNotPresentPaymentByApplicationRequestInternalCommand>
    {
        public Guid ApplicationId { get; set; }
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
