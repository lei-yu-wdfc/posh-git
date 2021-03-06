using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Commands.Wb.Uk
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.CreateBusinessFixedInstallmentLoanApplication </summary>
    [XmlRoot("CreateBusinessFixedInstallmentLoanApplication", Namespace = "Wonga.Payments.Commands.Wb.Uk", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateBusinessFixedInstallmentLoanApplication : MsmqMessage<CreateBusinessFixedInstallmentLoanApplication>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid BusinessPaymentCardId { get; set; }
        public Guid BusinessBankAccountId { get; set; }
        public Guid MainApplicantPaymentCardId { get; set; }
        public Guid MainApplicantBankAccountId { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Int32 Term { get; set; }
        public Decimal LoanAmount { get; set; }
        public Guid? PromoCodeId { get; set; }
        public String AffiliateId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
