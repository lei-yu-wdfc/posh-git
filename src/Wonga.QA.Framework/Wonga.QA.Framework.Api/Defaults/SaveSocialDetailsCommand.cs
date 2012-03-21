using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SaveSocialDetailsCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            Dependants = Get.RandomInt(0, 10);
            MaritalStatus = Get.RandomEnum<MaritalStatusEnum>();
            OccupancyStatus = Get.RandomEnum<OccupancyStatusEnum>();
        }
    }
}