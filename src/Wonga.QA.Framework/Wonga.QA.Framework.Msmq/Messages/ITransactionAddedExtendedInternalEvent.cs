using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ITransactionAddedExtendedInternal </summary>
    [XmlRoot("ITransactionAddedExtendedInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.InternalMessages.Messages.ITransactionAddedInternal,Wonga.Payments.PublicMessages.ITransactionAdded,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class ITransactionAddedExtendedInternalEvent : MsmqMessage<ITransactionAddedExtendedInternalEvent>
    {
        public Guid AccountId { get; set; }
        public DateTime PostedOn { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Int32 ApplicationInternalId { get; set; }
        public PaymentTransactionEnum TransactionType { get; set; }
        public PaymentTransactionScopeEnum TransactionScope { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
