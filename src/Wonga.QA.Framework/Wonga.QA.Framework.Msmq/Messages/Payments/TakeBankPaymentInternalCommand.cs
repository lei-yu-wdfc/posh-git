using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("TakeBankPaymentInternalMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class TakeBankPaymentInternalCommand : MsmqMessage<TakeBankPaymentInternalCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String BankAccount { get; set; }
        public String BankCode { get; set; }
        public DateTime TakePaymentOnDate { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
