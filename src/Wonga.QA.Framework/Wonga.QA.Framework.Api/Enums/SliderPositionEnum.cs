using System.ComponentModel;
namespace Wonga.QA.Framework.Api
{
    public enum SliderPositionEnum
    {
        [Description("Custom")]
        Custom,
        [Description("Default")]
        Default,
        [Description("Max")]
        Max,
        [Description("Min")]
        Min,
    }
}
