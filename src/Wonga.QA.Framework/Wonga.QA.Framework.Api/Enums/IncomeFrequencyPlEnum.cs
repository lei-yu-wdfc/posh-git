using System.ComponentModel;
namespace Wonga.QA.Framework.Api
{
    public enum IncomeFrequencyPlEnum
    {
        [Description("raz na tydzień")]
        raz_na_tydzień,
        [Description("raz na 2 tygodnie")]
        raz_na_2_tygodnie,
        [Description("raz na miesiąc")]
        raz_na_miesiąc,
        [Description("raz na kwartał")]
        raz_na_kwartał,
        [Description("raz na rok")]
        raz_na_rok,
    }
}
