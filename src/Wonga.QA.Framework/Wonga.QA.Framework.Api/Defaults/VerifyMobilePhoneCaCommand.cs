using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Ca
{
    public partial class VerifyMobilePhoneCaCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            VerificationId = Get.GetId();
            MobilePhone = "0720000000";
            Forename = "Forename";
        }
    }
}
