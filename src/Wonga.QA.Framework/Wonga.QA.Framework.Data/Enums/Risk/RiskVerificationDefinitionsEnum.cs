using System.ComponentModel;

namespace Wonga.QA.Framework.Data.Enums.Risk
{
    public enum RiskVerificationDefinitions
    {
        [Description("CreditBureauEcbsScoreIsAcceptableVerification")] CreditBureauEcbsScoreIsAcceptableVerification,
        [Description("CreditBureauDataIsAvailableVerification")] CreditBureauDataIsAvailableVerification,
        [Description("CreditBureauCustomerIsSolventVerification")] CreditBureauCustomerIsSolventVerification,
        [Description("CreditBureauCustomerIsAliveVerification")] CreditBureauCustomerIsAliveVerification,
        [Description("DateOfBirthIsCorrectVerification")] DateOfBirthIsCorrectVerification,
        [Description("BusinessDataAvailableInGraydonVerification")] BusinessDataAvailableInGraydonVerification,
        [Description("GraydonBusinessIsTradingVerification")] GraydonBusinessIsTradingVerification,
        [Description("GraydonPaymentScoreVerification")] GraydonPaymentScoreVerification,
        [Description("GraydonAugurScoreVerification")] GraydonAugurScoreVerification,
        [Description("CreditBureauCifasFraudCheckVerification")] CreditBureauCifasFraudCheckVerification,
        [Description("CardPaymentPaymentCardIsValidVerification")] CardPaymentPaymentCardIsValidVerification,
        [Description("CallValidatePaymentCardIsValidVerification")] CallValidatePaymentCardIsValidVerification,
        [Description("ExperianPaymentCardIsValidVerification")] ExperianPaymentCardIsValidVerification,
        [Description("LiveIdvDobIsCorrectVerification")] LiveIdvDobIsCorrectVerification,
        [Description("RepaymentVerification")] RepaymentVerification,
        [Description("MainApplicantMatchesBusinessBureauDataVerification")] MainApplicantMatchesBusinessBureauDataVerification,
        [Description("MainApplicantDurationAcceptableVerification")] MainApplicantDurationAcceptableVerification,
        [Description("BusinessDateOfIncorporationAcceptableVerification")] BusinessDateOfIncorporationAcceptableVerification,
        [Description("NumberOfDirectorsMatchesBusinessBureauDataVerification")] NumberOfDirectorsMatchesBusinessBureauDataVerification,
        [Description("GuarantorNamesMatchBusinessBureauDataVerification")] GuarantorNamesMatchBusinessBureauDataVerification,
        [Description("MobilePhoneIsUniqueVerification")] MobilePhoneIsUniqueVerification,
		[Description("DoNotRelendVerification")] DoNotRelendVerification,
		[Description("FraudBlacklistVerification")] FraudBlacklistVerification
    }
}
