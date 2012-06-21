using System.ComponentModel;
namespace Wonga.QA.Framework.Api.Enums
{
    public enum PaymentMethodEnum
    {
        [Description("BankAccount")]
        BankAccount,
        [Description("ETransfer")]
        ETransfer,
        [Description("PayPal")]
        PayPal,
    }
}
