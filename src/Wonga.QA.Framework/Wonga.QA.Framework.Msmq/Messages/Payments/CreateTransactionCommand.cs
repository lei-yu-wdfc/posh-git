using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("CreateTransaction", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class CreateTransactionCommand : MsmqMessage<CreateTransactionCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExternalId { get; set; }
        public Guid? ComponentTransactionId { get; set; }
        public DateTime PostedOn { get; set; }
        public PaymentTransactionScopeEnum Scope { get; set; }
        public PaymentTransactionEnum Type { get; set; }
        public Decimal Amount { get; set; }
        public Decimal Mir { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public String Reference { get; set; }
        public PaymentTransactionSourceEnum Source { get; set; }
        public Int32? UserId { get; set; }
    }
}
