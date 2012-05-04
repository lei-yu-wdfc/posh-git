namespace Wonga.QA.Tests.BankGateway.Enums
{
    public enum BankGatewayTransactionStatus
    {
        New = 1,
        Open = 2,
        InProgress = 3,
        Paid = 4,
        Failed = 5,
        Expired = 6,
        Pending = 7,
        SendFailed = 8,
        ChargedBack = 9
    }
}