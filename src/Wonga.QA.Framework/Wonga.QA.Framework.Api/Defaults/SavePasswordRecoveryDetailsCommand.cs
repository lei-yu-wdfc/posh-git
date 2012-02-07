using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SavePasswordRecoveryDetailsCommand

    {
        public override void Default()
        {
            AccountId = Data.GetId();
            SecretAnswer = Data.RandomString(30);
            SecretQuestion = Data.RandomString(30);
        }
    }
}