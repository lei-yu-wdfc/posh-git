using System.ComponentModel;
namespace Wonga.QA.Framework.Cs
{
    public enum PaymentTransactionScopeEnum
    {
        [Description("Other")]
        Other,
        [Description("Debit")]
        Debit,
        [Description("Credit")]
        Credit,
    }
}
