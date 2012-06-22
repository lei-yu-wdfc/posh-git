using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Za
{
    /// <summary> Wonga.Payments.Za.CreateFixedTermLoanApplication </summary>
    [XmlRoot("CreateFixedTermLoanApplication", Namespace = "Wonga.Payments.Za", DataType = "")]
    public partial class CreateFixedTermLoanApplicationZaCommand : MsmqMessage<CreateFixedTermLoanApplicationZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? PaymentCardId { get; set; }
        public Guid BankAccountId { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public DateTime PromiseDate { get; set; }
        public Decimal LoanAmount { get; set; }
        public Guid? PromoCodeId { get; set; }
        public String AffiliateId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
