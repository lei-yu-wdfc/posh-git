using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.Wb.RiskCreateBusinessFixedInstallmentLoanApplication </summary>
    [XmlRoot("RiskCreateBusinessFixedInstallmentLoanApplication")]
    public partial class RiskCreateBusinessFixedInstallmentLoanApplicationWbCommand : ApiRequest<RiskCreateBusinessFixedInstallmentLoanApplicationWbCommand>
    {
        public Object AccountId { get; set; }
        public Object OrganisationId { get; set; }
        public Object ApplicationId { get; set; }
        public Object BusinessPaymentCardId { get; set; }
        public Object BusinessBankAccountId { get; set; }
        public Object MainApplicantPaymentCardId { get; set; }
        public Object MainApplicantBankAccountId { get; set; }
        public Object Currency { get; set; }
        public Object LoanAmount { get; set; }
    }
}
