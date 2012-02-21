using System.ComponentModel;

namespace Wonga.QA.Framework.Api
{
    public enum CheckpointDefinitionEnum
    {
        [Description("Credit Bureau Score is acceptable")] CreditBureauScoreisacceptable,
        [Description("Application terms are acceptable for business")] Applicationtermsareacceptableforbusiness,
        [Description("Applicant is alive")] Applicantisalive,
        [Description("Customer is solvent")] Customerissolvent,
        [Description("Hardware blacklist check")] Hardwareblacklistcheck,
        [Description("CIFAS fraud check")] CIFASfraudcheck,
        [Description("Application data blacklist check")] Applicationdatablacklistcheck,
        [Description("Fraud list check")] Fraudlistcheck,
        [Description("Bank account matches the applicant")] Bankaccountmatchestheapplicant,
        [Description("Credit bureau data is available")] Creditbureaudataisavailable,
        [Description("Date of birth is correct")] Dateofbirthiscorrect,
        [Description("Applicant history is acceptable")] Applicanthistoryisacceptable,
        [Description("Customer is employed")] Customerisemployed,
        [Description("Customer has provided correct forename & surname")] Customerhasprovidedcorrectforenamesurname,
        [Description("Automated Fraud Attempt check")] AutomatedFraudAttemptcheck,
        [Description("Monthly income limit check")] Monthlyincomelimitcheck,
        [Description("Suspicious activity check")] Suspiciousactivitycheck,
        [Description("Payment Card is valid")] PaymentCardisvalid,
        [Description("Repayment prediction check")] Repaymentpredictioncheck,
        [Description("User assisted fraud check")] Userassistedfraudcheck,
        [Description("Applicant is not minor")] Applicantisnotminor,
        [Description("Bank account accept debits")] Bankaccountacceptdebits,
        [Description("Ability to verify personal data")] Abilitytoverifypersonaldata,
        [Description("Business Bureau Data Is Available")] BusinessBureauDataIsAvailable,
        [Description("BusinessIsCurrentlyTrading")] BusinessIsCurrentlyTrading,
        [Description("BusinessPaymentScoreIsAcceptable")] BusinessPaymentScoreIsAcceptable,
        [Description("BusinessPerformanceScoreIsAcceptaple")] BusinessPerformanceScoreIsAcceptaple
    }
}
