using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Enums.Common.TimeZone;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Events
{
    /// <summary> Wonga.Payments.InternalMessages.Events.IRepayLoanViaCardRequestStarted </summary>
    [XmlRoot("IRepayLoanViaCardRequestStarted", Namespace = "Wonga.Payments.InternalMessages.Events", DataType = "")]
    public partial class IRepayLoanViaCardRequestStartedEvent : MsmqMessage<IRepayLoanViaCardRequestStartedEvent>
    {
        public Guid SagaId { get; set; }
        public Guid ApplicationGuid { get; set; }
        public Int32 ApplicationId { get; set; }
        public Guid PaymentCardGuid { get; set; }
        public Int32 PaymentCardId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public MsTimeZoneEnum CustomerTimeZone { get; set; }
    }
}
