using System.ComponentModel;
namespace Wonga.QA.Framework.Api.Enums
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
        [Description("TwiceMonthly15thAnd30th")]
        TwiceMonthly15thAnd30th,
        
    }
}
