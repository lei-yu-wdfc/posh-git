using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class VerifyFixedTermLoanCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            ApplicationId = Data.GetId();
        }
    }
}
