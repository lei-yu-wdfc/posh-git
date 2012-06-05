using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Ca.CreateFixedTermLoanApplicationCa </summary>
    [XmlRoot("CreateFixedTermLoanApplicationCa", Namespace = "Wonga.Payments.Ca", DataType = "")]
    public partial class CreateFixedTermLoanApplicationCaCommand : MsmqMessage<CreateFixedTermLoanApplicationCaCommand>
    {
        public ProvinceEnum Province { get; set; }
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
