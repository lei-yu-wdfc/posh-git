using System.ComponentModel;
namespace Wonga.QA.Framework.Api
{
    public enum EmploymentStatusPlEnum
    {
        [Description("Umowa o pracę na czas nieokreślony")]
        Umowa_o_pracę_na_czas_nieokreślony,
        [Description("Umowa o pracę na czas określony")]
        Umowa_o_pracę_na_czas_określony,
        [Description("Umowa zlecenie/Umowa o dzieło")]
        Umowa_zlecenieUmowa_o_dzieło,
        [Description("Działalność gospodarcza")]
        Działalność_gospodarcza,
        [Description("Student")]
        Student,
        [Description("Pracownik Służb Mundurowych")]
        Pracownik_Służb_Mundurowych,
        [Description("Emeryt/Rencista")]
        EmerytRencista,
        [Description("Gospodyni domowa")]
        Gospodyni_domowa,
        [Description("Bezrobotny")]
        Bezrobotny,
    }
}
