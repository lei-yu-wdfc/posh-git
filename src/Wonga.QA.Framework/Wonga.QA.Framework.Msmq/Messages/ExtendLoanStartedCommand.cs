using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ExtendLoanStartedMessage </summary>
    [XmlRoot("ExtendLoanStartedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ExtendLoanStartedCommand : MsmqMessage<ExtendLoanStartedCommand>
    {
        public Guid AccountId { get; set; }
        public Int32 LoanExtensionId { get; set; }
        public Guid LoanExtensionExternalId { get; set; }
        public Decimal Amount { get; set; }
        public Object CV2 { get; set; }
        public DateTime LocalTime { get; set; }
        public DateTime ExtensionDate { get; set; }
        public String CardType { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public DateTime ExpiryDate { get; set; }
        public String HolderName { get; set; }
        public DateTime? StartDate { get; set; }
        public Guid PaymentCardId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
