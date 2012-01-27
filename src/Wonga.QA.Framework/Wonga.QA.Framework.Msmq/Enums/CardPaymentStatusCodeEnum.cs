namespace Wonga.QA.Framework.Msmq
{
    public enum CardPaymentStatusCodeEnum
    {
        Authorized = 1,
        FraudDetected = 2,
        ExpiredCard = 4,
        AmountInvalid = 8,
        RequiredParamsNotPresent = 16,
        DuplicatePayment = 32,
        InvalidPayment = 40,
        StartDateInvalid = 64,
        ExpiryDateInvalid = 128,
        IssueNumberInvalid = 256,
        CardNumberInvalid = 512,
        CardTypeInvalid = 1024,
        HolderNameNotPresent = 2048,
        CV2InvalidOrNotPresent = 4096,
        InvalidCard = 8148,
        OneTimeUsageCard = 8192,
        CommunicationProblem = 16384,
        MerchantNotFound = 32768,
        MerchantForCardTypeNotFound = 65536,
        MerchantForCurrencyNotFound = 131072,
        TransactionTimedOut = 262144,
        SignatureInvalid = 524288,
        RequestDeclined = 1032192,
    }
}
