using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Wb.Uk
{
    public partial class CreateBusinessFixedInstallmentLoanApplicationWbUkCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            ApplicationId = Get.GetId();
            Currency = CurrencyCodeEnum.GBP;
            LoanAmount = 10000;
            BusinessBankAccountId = Get.GetId();
            BusinessPaymentCardId = Get.GetId();
            MainApplicantPaymentCardId = Get.GetId();
            MainApplicantBankAccountId = Get.GetId();
            Term = 20;
            OrganisationId = Get.GetId();
        }
    }
}
