using System.ComponentModel;
namespace Wonga.QA.Framework.Cs
{
    public enum EmploymentStatusEnum
    {
        [Description("EmployedFullTime")]
        EmployedFullTime,
        [Description("EmployedPartTime")]
        EmployedPartTime,
        [Description("EmployedTemporary")]
        EmployedTemporary,
        [Description("SelfEmployed")]
        SelfEmployed,
        [Description("Student")]
        Student,
        [Description("HomeMaker")]
        HomeMaker,
        [Description("Retired")]
        Retired,
        [Description("Unemployed")]
        Unemployed,
        [Description("OnBenefits")]
        OnBenefits,
        [Description("ArmedForces")]
        ArmedForces,
    }
}
