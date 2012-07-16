using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.RepayPayLaterApplicationMessage </summary>
    [XmlRoot("RepayPayLaterApplicationMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "")]
    public partial class RepayPayLaterApplicationMessage : MsmqMessage<RepayPayLaterApplicationMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Int64 PaymentReference { get; set; }
        public Decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ValueDate { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid SagaId { get; set; }
    }
}
