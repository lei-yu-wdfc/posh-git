using System.ComponentModel;
namespace Wonga.QA.Framework.Api
{
    public enum AdditionalPaymentCardDetailsEnum
    {
        [Description("AdditionalNumber")]
        AdditionalNumber,
        [Description("AdditionalName")]
        AdditionalName,
        [Description("AdditionalExpireDate")]
        AdditionalExpireDate,
    }
}
