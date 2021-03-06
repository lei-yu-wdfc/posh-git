using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands.Wb
{
    /// <summary> Wonga.Risk.Commands.Wb.RiskCreateBusinessFixedInstallmentLoanApplication </summary>
    [XmlRoot("RiskCreateBusinessFixedInstallmentLoanApplication", Namespace = "Wonga.Risk.Commands.Wb", DataType = "" )
    , SourceAssembly("Wonga.Risk.Commands.Wb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RiskCreateBusinessFixedInstallmentLoanApplication : MsmqMessage<RiskCreateBusinessFixedInstallmentLoanApplication>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid BusinessPaymentCardId { get; set; }
        public Guid BusinessBankAccountId { get; set; }
        public Guid MainApplicantPaymentCardId { get; set; }
        public Guid MainApplicantBankAccountId { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Int32 Term { get; set; }
        public Decimal LoanAmount { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
