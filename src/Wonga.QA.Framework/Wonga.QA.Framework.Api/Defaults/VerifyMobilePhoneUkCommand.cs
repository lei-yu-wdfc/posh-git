using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class VerifyMobilePhoneUkCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            VerificationId = Data.GetId();
            MobilePhone = "07800000000";
            Forename = "Forename";
        }
    }
}
