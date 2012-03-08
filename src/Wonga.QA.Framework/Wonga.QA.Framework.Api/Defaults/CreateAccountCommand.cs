using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class CreateAccountCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            Login = Data.RandomEmail();
            Password = Data.GetPassword();
        }
    }
}