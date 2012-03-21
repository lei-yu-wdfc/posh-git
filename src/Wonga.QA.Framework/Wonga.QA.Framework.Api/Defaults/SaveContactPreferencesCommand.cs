using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SaveContactPreferencesCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            AcceptMarketingContact = Get.RandomBoolean();
        }
    }
}