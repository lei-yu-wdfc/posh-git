using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SavePasswordRecoveryDetailsZaCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            SecretAnswer = Get.RandomString(30);
            SecretQuestion = Get.RandomString(30);
        }
    }
}