using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Commands.Uk
{
    public partial class SignApplicationPayLaterCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            ApplicationId = Get.GetId();
        }
    }
}
