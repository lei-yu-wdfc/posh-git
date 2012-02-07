using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SaveSocialDetailsCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            Dependants = Data.RandomInt(0, 10);
            MaritalStatus = Data.RandomEnum<MaritalStatusEnum>();
            OccupancyStatus = Data.RandomEnum<OccupancyStatusEnum>();
        }
    }
}