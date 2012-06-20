using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.Ca;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.RiskCreateFixedTermLoanApplication </summary>
    [XmlRoot("RiskCreateFixedTermLoanApplication", Namespace = "Wonga.Risk", DataType = "")]
    public partial class RiskCreateFixedTermLoanApplicationCommand : MsmqMessage<RiskCreateFixedTermLoanApplicationCommand>
    {
        public Guid AccountId { get; set; }
        public Guid? PaymentCardId { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public DateTime PromiseDate { get; set; }
        public Decimal LoanAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
