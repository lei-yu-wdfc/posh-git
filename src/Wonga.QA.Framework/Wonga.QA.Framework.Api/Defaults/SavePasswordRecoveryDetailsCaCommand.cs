using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands.Ca
{
    public partial class SavePasswordRecoveryDetailsCaCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            SecretAnswer = Get.RandomString(30);
            SecretQuestion = Get.RandomString(30);
        }
    }
}