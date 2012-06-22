using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
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