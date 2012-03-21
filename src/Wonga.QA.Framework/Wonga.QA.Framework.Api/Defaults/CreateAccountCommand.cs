using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class CreateAccountCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            Login = Get.RandomEmail();
            Password = Get.GetPassword();
        }
    }
}