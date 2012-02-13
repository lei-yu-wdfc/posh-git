using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class CreateBusinessFixedInstallmentLoanApplicationWbUkCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            ApplicationId = Data.GetId();
            Currency = CurrencyCodeEnum.GBP;
            LoanAmount = 10000;
            BusinessBankAccountId = Data.GetId();
            BusinessPaymentCardId = Data.GetId();
            MainApplicantPaymentCardId = Data.GetId();
            MainApplicantBankAccountId = Data.GetId();
            Term = 20;
            OrganisationId = Data.GetId();
        }
    }
}
