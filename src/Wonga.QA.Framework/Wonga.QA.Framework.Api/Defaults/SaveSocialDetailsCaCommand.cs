using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Ca
{
    public partial class SaveSocialDetailsCaCommand
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