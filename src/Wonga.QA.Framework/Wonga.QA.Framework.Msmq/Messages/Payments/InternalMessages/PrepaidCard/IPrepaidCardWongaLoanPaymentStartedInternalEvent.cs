using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.Ca;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.PrepaidCard
{
    /// <summary> Wonga.Payments.InternalMessages.PrepaidCard.IPrepaidCardWongaLoanPaymentStartedInternalEvent </summary>
    [XmlRoot("IPrepaidCardWongaLoanPaymentStartedInternalEvent", Namespace = "Wonga.Payments.InternalMessages.PrepaidCard", DataType = "")]
    public partial class IPrepaidCardWongaLoanPaymentStartedInternalEvent : MsmqMessage<IPrepaidCardWongaLoanPaymentStartedInternalEvent>
    {
        public Guid SagaId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Guid CustomerExternalId { get; set; }
        public Guid CardDetailsExternalId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
