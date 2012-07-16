using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.PrepaidCard
{
    /// <summary> Wonga.Payments.InternalMessages.PrepaidCard.IPrepaidCardWongaLoanDebitPaymentStartedInternalEvent </summary>
    [XmlRoot("IPrepaidCardWongaLoanDebitPaymentStartedInternalEvent", Namespace = "Wonga.Payments.InternalMessages.PrepaidCard", DataType = "")]
    public partial class IPrepaidCardWongaLoanDebitPaymentStartedInternalEvent : MsmqMessage<IPrepaidCardWongaLoanDebitPaymentStartedInternalEvent>
    {
        public Guid SagaId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Guid CustomerExternalId { get; set; }
        public Guid CardDetailsExternalId { get; set; }
        public Guid DebitCardExternalId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime InitiatedOn { get; set; }
        public DateTime RequestedOn { get; set; }
    }
}
