using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Api
{
    public enum VerificationDefinitionsEnum
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
    }
}
