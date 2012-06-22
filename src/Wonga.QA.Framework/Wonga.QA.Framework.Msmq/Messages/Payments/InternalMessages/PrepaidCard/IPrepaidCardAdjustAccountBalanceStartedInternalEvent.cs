using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.PrepaidCard
{
    /// <summary> Wonga.Payments.InternalMessages.PrepaidCard.IPrepaidCardAdjustAccountBalanceStartedInternalEvent </summary>
    [XmlRoot("IPrepaidCardAdjustAccountBalanceStartedInternalEvent", Namespace = "Wonga.Payments.InternalMessages.PrepaidCard", DataType = "")]
    public partial class IPrepaidCardAdjustAccountBalanceStartedInternalEvent : MsmqMessage<IPrepaidCardAdjustAccountBalanceStartedInternalEvent>
    {
        public Guid SagaId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Guid CustomerExternalId { get; set; }
        public Guid CardDetailsExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
        public String Reason { get; set; }
        public Guid BalanceAdjustmentId { get; set; }
    }
}
