using System.ComponentModel;
namespace Wonga.QA.Framework.Api.Enums
{
    public enum DeclineAdviceKeyEnum
    {
        [Description("GeneralResolutionAdvice")]
        GeneralResolutionAdvice,
        [Description("BankAccountMatchAdvice")]
        BankAccountMatchAdvice,
        [Description("CreditBureauMissDetailsAdvice")]
        CreditBureauMissDetailsAdvice,
        [Description("CheckCreditHistoryAdvice")]
        CheckCreditHistoryAdvice,
        [Description("CheckDateOfBirthAdvice")]
        CheckDateOfBirthAdvice,
        [Description("CheckFinancialHealthAdvice")]
        CheckFinancialHealthAdvice,
        [Description("CheckPaymentCardDetailsAdvice")]
        CheckPaymentCardDetailsAdvice,
        [Description("RepaymentAbilityAdvice")]
        RepaymentAbilityAdvice,
    }
}
