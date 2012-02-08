using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class CreateBusinessFixedInstallmentLoanApplicationWbUkCommand
    {
        public override void Default()
        {
            AffiliateId = "";
            AccountId = Data.GetId();
            ApplicationId = Data.GetId();
            Currency = CurrencyCodeEnum.GBP;
            LoanAmount = 10000;
            PromoCodeId = null;
            BusinessBankAccountId = Data.RandomInt(9999);
            BusinessPaymentCardId = Data.RandomInt(9999);
            NumberOfWeeks = 20;
            OrganisationId = Data.GetId();
        }
    }
}
