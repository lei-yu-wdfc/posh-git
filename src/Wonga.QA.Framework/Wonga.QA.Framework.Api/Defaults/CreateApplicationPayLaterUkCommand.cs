using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Commands.Uk
{
    public partial class CreateApplicationPayLaterUkCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            ApplicationId = Get.GetId();
            MerchantId = Get.GetId();
            MerchantReference = "MerchantRef";
            MerchantOrderId = Get.GetId();
            TotalAmount = Get.GetLoanAmount();
            Currency = CurrencyCodeEnum.GBP;
            PostCode = Get.GetPostcode();
        }
    }
}
