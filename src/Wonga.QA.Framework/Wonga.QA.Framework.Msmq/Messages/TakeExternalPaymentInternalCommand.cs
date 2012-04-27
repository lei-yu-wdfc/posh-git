using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.TakeExternalPaymentInternalMessage </summary>
    [XmlRoot("TakeExternalPaymentInternalMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "")]
    public partial class TakeExternalPaymentInternalCommand : MsmqMessage<TakeExternalPaymentInternalCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ExternalCardId { get; set; }
        public String CardType { get; set; }
        public Object CardNumber { get; set; }
        public Object CV2 { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public String IssueNo { get; set; }
        public String HolderName { get; set; }
        public String AddressLine1 { get; set; }
        public String AddressLine2 { get; set; }
        public String Town { get; set; }
        public String County { get; set; }
        public String PostCode { get; set; }
        public String Country { get; set; }
        public RepaymentRequestEnum RepaymentRequestType { get; set; }
        public Guid? PaymentExternalId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
