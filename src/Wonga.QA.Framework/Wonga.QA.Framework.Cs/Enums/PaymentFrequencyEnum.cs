using System.ComponentModel;
namespace Wonga.QA.Framework.Cs
{
    public enum PaymentFrequencyEnum
    {
        [Description("Weekly")]
        Weekly,
        [Description("Every2Weeks")]
        Every2Weeks,
        [Description("Every4Weeks")]
        Every4Weeks,
        [Description("Monthly")]
        Monthly,
    }
}
