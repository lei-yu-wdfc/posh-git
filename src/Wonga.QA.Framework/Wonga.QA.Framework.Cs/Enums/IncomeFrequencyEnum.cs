using System.ComponentModel;
namespace Wonga.QA.Framework.Cs
{
    public enum IncomeFrequencyEnum
    {
        [Description("Monthly")]
        Monthly,
        [Description("TwiceMonthly")]
        TwiceMonthly,
        [Description("Weekly")]
        Weekly,
        [Description("BiWeekly")]
        BiWeekly,
        [Description("LastDayOfMonth")]
        LastDayOfMonth,
        [Description("LastFridayOfMonth")]
        LastFridayOfMonth,
    }
}
