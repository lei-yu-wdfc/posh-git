using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("CreateBusinessFixedInstallmentLoanApplication", Namespace = "Wonga.Payments.Commands.Wb.Uk", DataType = "")]
    public partial class CreateBusinessFixedInstallmentLoanApplicationWbUkCommand : MsmqMessage<CreateBusinessFixedInstallmentLoanApplicationWbUkCommand>
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
