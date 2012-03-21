using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class VerifyMobilePhoneUkCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            VerificationId = Get.GetId();
            MobilePhone = "07800000000";
            Forename = "Forename";
        }
    }
}
