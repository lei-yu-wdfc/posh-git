using System.ComponentModel;
namespace Wonga.QA.Framework.Api
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
