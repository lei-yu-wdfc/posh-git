using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class VerifyMobilePhoneZaCommand
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
