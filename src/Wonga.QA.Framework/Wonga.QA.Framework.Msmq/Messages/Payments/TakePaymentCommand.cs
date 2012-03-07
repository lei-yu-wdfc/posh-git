using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.TakePaymentMessage </summary>
    [XmlRoot("TakePaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class TakePaymentCommand : MsmqMessage<TakePaymentCommand>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public String CardType { get; set; }
        public String HolderName { get; set; }
        public Object CardNumber { get; set; }
        public Object CV2 { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public String IssueNo { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
