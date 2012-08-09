using System.ComponentModel;
namespace Wonga.QA.Framework.Api.Enums
{
    public enum EducationalLevelsEnum
    {
        [Description("None")]
        None,
        [Description("Primary")]
        Primary,
        [Description("JuniorHighSchool")]
        JuniorHighSchool,
        [Description("HighSchool")]
        HighSchool,
        [Description("VocationalSchool")]
        VocationalSchool,
        [Description("HigherEducationLicentiate")]
        HigherEducationLicentiate,
        [Description("HigherEducationFull")]
        HigherEducationFull,
        [Description("Postgraduate")]
        Postgraduate,
    }
}
