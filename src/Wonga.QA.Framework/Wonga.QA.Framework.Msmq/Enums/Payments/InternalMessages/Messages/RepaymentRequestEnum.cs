namespace Wonga.QA.Framework.Msmq.Enums.Payments.InternalMessages.Messages
{
    public enum RepaymentRequestEnum
    {
        ScheduledPayment = 0,
        RepayLoan = 1,
        CsAgent = 2,
        ExtendLoanPartPayment = 3,
        CustomerViaMyAccount = 4,
        PayLaterTransactionFee = 5,
    }
}
