using System.ComponentModel;

namespace Wonga.QA.Framework.Api
{
    public enum RiskCheckpointDefinitionEnum
    {
        [Description("Credit Bureau Score is acceptable")] CreditBureauScoreisacceptable,
        [Description("Application terms are acceptable for business")] ApplicationTermsAreAcceptableForBusiness,
        [Description("Applicant is alive")] ApplicantIsAlive,
        [Description("Customer is solvent")] CustomerIsSolvent,
        [Description("Hardware blacklist check")] HardwareBlacklistCheck,
        [Description("CIFAS fraud check")] CIFASFraudCheck,
        [Description("Application data blacklist check")] ApplicationDataBlacklistCheck,
        [Description("Fraud list check")] FraudListCheck,
        [Description("Bank account matches the applicant")] BankAccountMatchesTheApplicant,
        //[Description("Bank account is valid")]BankAccountIsValid,
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
        [Description("BusinessPerformanceScoreIsAcceptaple")] BusinessPerformanceScoreIsAcceptaple,
        [Description("Main Applicant Matches Business Bureau Data")] MainApplicantMatchesBusinessBureauData,
        [Description("Main Applicant Duration Acceptable")] MainApplicantDurationAcceptable,
        [Description("Business Date Of Incorporation Acceptable")] BusinessDateOfIncorporationAcceptable,
        [Description("Number Of Directors Matches Business Bureau Data")] NumberOfDirectorsMatchesBusinessBureauData,
        [Description("Guarantor Names Match Business Bureau Data")] GuarantorNamesMatchBusinessBureauData,
        [Description("There are to many active loans at same address")] TooManyLoansAtAddress
    }
}
