using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.PrepaidCard.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToMakePromoPayment </summary>
    [XmlRoot("IWantToMakePromoPayment", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToMakePromoPayment : MsmqMessage<IWantToMakePromoPayment>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PrepaidCardId { get; set; }
        public Decimal Amount { get; set; }
        public PromoPaymentEnum PromoPaymentType { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public String BankAccount { get; set; }
        public String BankCode { get; set; }
        public String BankAccountType { get; set; }
        public Guid SenderRefference { get; set; }
        public Guid SagaId { get; set; }
    }
}
