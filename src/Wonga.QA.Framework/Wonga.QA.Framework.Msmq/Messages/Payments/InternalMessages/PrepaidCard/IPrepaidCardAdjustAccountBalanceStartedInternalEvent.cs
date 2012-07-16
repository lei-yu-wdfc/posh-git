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
        public DateTime InitiatedOn { get; set; }
        public DateTime RequestedOn { get; set; }
        public String Reason { get; set; }
        public Guid BalanceAdjustmentId { get; set; }
        public String SalesForceUser { get; set; }
        public Guid ApplicationId { get; set; }
        public String BankAccount { get; set; }
        public String BankCode { get; set; }
        public String BankAccountType { get; set; }
        public Guid SenderRefference { get; set; }
    }
}
