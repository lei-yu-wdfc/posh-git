using System.ComponentModel;
namespace Wonga.QA.Framework.Api.Enums
{
    public enum VehicleOwnerEnum
    {
        [Description("Yes")]
        Yes,
        [Description("Lease")]
        Lease,
        [Description("Business")]
        Business,
        [Description("No")]
        No,
    }
}
