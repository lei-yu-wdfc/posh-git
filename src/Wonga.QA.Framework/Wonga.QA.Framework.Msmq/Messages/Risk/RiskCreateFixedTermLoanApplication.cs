using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.RiskCreateFixedTermLoanApplication </summary>
    [XmlRoot("RiskCreateFixedTermLoanApplication", Namespace = "Wonga.Risk", DataType = "" )
    , SourceAssembly("Wonga.Risk.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RiskCreateFixedTermLoanApplication : MsmqMessage<RiskCreateFixedTermLoanApplication>
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
