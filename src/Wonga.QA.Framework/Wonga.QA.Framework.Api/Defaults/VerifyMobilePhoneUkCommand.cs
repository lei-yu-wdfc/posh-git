using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
    public partial class VerifyMobilePhoneUkCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            VerificationId = Get.GetId();
            MobilePhone = "07700900001";
            Forename = "Forename";
        }
    }
}
