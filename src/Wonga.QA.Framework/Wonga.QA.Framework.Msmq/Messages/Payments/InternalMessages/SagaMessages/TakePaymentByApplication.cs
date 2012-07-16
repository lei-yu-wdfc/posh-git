using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.TakePaymentByApplication </summary>
    [XmlRoot("TakePaymentByApplication", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "")]
    public partial class TakePaymentByApplication : MsmqMessage<TakePaymentByApplication>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
        public String CardType { get; set; }
        public String HolderName { get; set; }
        public Object CV2 { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public String IssueNo { get; set; }
        public Object BillingAddress { get; set; }
        public RepaymentRequestEnum RepaymentRequestType { get; set; }
        public Guid? PaymentExternalId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
