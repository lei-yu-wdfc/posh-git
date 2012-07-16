using System.ComponentModel;
namespace Wonga.QA.Framework.Api.Enums
{
    public enum MaritalStatusEnum
    {
        [Description("Married")]
        Married,
        [Description("Single")]
        Single,
        [Description("Divorced")]
        Divorced,
        [Description("Widowed")]
        Widowed,
        [Description("LivingTogether")]
        LivingTogether,
        [Description("Separated")]
        Separated,
        [Description("Other")]
        Other,
    }
}
