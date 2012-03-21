using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class CompleteMobilePhoneVerificationCommand
    {
        public override void Default()
        {
            VerificationId = Get.GetId();
            Pin = "0000";
        }
    }
}