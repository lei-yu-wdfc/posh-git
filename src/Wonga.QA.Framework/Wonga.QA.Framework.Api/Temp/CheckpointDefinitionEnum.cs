using System.ComponentModel;

namespace Wonga.QA.Framework.Api
{
    public enum CheckpointDefinitionEnum
    {
        [Description("Credit Bureau Score is acceptable")] CreditBureauScoreisacceptable,
        [Description("Application terms are acceptable for business")] ApplicationTermsAreAcceptableForBusiness,
        [Description("Applicant is alive")] ApplicantIsAlive,
        [Description("Customer is solvent")] CustomerIsSolvent,
        [Description("Hardware blacklist check")] HardwareBlacklistCheck,
        [Description("CIFAS fraud check")] CIFASFraudCheck,
        [Description("Hardware blacklist check")] Applicationdatablacklistcheck,
        [Description("Fraud list check")] FraudListCheck,
		[Description("Bank account matches the applicant")]BankAccountMatchesTheApplicant,
        [Description("Credit bureau data is available")] CreditBureauDataIsAvailable,
        [Description("Date of birth is correct")] DateOfBirthIsCorrect,
        [Description("Applicant history is acceptable")] ApplicantHistoryIsAcceptable,
        [Description("Customer is employed")] CustomerIsEmployed,
        [Description("Customer has provided correct forename & surname")] CustomerHasProvidedCorrectForenameSurname,
        [Description("Automated Fraud Attempt check")] AutomatedFraudAttemptcheck,
        [Description("Monthly income limit check")] MonthlyIncomeLimitCheck,
        [Description("Suspicious activity check")] SuspiciousActivityCheck,
        [Description("Payment Card is valid")] PaymentCardIsValid,
        [Description("Repayment prediction check")] RepaymentPredictionCheck,
        [Description("User assisted fraud check")] UserAssistedFraudCheck,
        [Description("Applicant is not minor")] ApplicantIsNotMinor,
        [Description("Bank account accept debits")] BankAccountAcceptDebits,
        [Description("Ability to verify personal data")] AbilityToVerifyPersonalData,
        [Description("Business Bureau Data Is Available")] BusinessBureauDataIsAvailable,
        [Description("BusinessIsCurrentlyTrading")] BusinessIsCurrentlyTrading,
        [Description("BusinessPaymentScoreIsAcceptable")] BusinessPaymentScoreIsAcceptable,
        [Description("BusinessPerformanceScoreIsAcceptaple")] BusinessPerformanceScoreIsAcceptaple
    }
}
