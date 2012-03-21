using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class VerifyMainBusinessApplicantWbCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            ApplicationId = Get.GetId();
        }
    }
}
