using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    public partial class RiskCreatePayLaterApplicationUkCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            ApplicationId = Get.GetId();
            PaymentCardId = Get.GetId();
            TotalAmount = Get.GetLoanAmount();
        }
    }
}