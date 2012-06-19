using System.ComponentModel;
namespace Wonga.QA.Framework.Api
{
    public enum OccupancyStatusEnum
    {
        [Description("OwnerOccupier")]
        OwnerOccupier,
        [Description("LivingWithParents")]
        LivingWithParents,
        [Description("TenantFurnished")]
        TenantFurnished,
        [Description("TenantUnfurnished")]
        TenantUnfurnished,
        [Description("CouncilTenant")]
        CouncilTenant,
        [Description("Tenant")]
        Tenant,
        [Description("JointOwner")]
        JointOwner,
        [Description("Other")]
        Other,
        [Description("SocialHousing")]
        SocialHousing,
    }
}
