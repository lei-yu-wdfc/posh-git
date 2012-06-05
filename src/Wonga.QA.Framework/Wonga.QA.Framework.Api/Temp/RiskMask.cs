﻿
namespace Wonga.QA.Framework.Api
{
	public enum RiskMask
	{
        TESTBankAccountHistoryIsAcceptable,
        TESTPaymentCardHistoryIsAcceptable,
		TESTTransUnionandBank,
		TESTTransUnion,
		TESTEmployedMask,
		TESTCardMask,
		TESTCardBankMask,
		TESTAll,
		TESTExcludeVerification,
		TESTBlacklist,
		TESTIsAlive,
		TESTDateOfBirth,
		TESTIsSolvent,
		TESTMonthlyIncome,
		TESTAccountNumberApplicationsAcceptable,
		TESTCustomerHistoryIsAcceptable,
		TESTApplicationElementNotOnBlacklist,
		TESTBankAccountMatchedToApplicant,
		TESTDirectFraud,
		TESTApplicationElementNotCIFASFlagged,
		TESTCreditBureauDataIsAvailable,
		TESTApplicantIsNotDeceased,
		TESTCustomerIsEmployed,
		TESTApplicantIsSolvent,
		TESTCustomerDateOfBirthIsCorrect,
		TESTCustomerDateOfBirthIsCorrectSME,
		TESTFraudScorePositive,
		TESTDirectFraudCheck,
		TESTCreditBureauScoreIsAcceptable,
		TESTApplicationElementNotOnCSBlacklist,
		TESTApplicationDeviceNotOnBlacklist,
		TESTDeviceNotOnBlacklist,
		TESTMonthlyIncomeEnoughForRepayment,
		TESTPaymentCardIsValid,
		TESTRepaymentPredictionPositive,
		TESTReputationtPredictionPositive,
		TESTNoSuspiciousApplicationActivity,
		TESTCallValidateBankAccountMatchedToApplicant,
		TESTCallValidatePaymentCardIsValid,
		TESTExperianBankAccountMatchedToApplicant,
		TESTExperianPaymentCardIsValid,
		TESTRiskBankAccountMatchedToApplicant,
		TESTRiskPaymentCardIsValid,
		TESTRiskFraudScorePositive,
		TESTExperianCreditBureauDataIsAvailable,
		TESTExperianApplicationElementNotCIFASFlagged,
		TESTExperianApplicantIsNotDeceased,
		TESTExperianApplicantIsSolvent,
		TESTExperianCustomerDateOfBirthIsCorrect,
		TESTExperianCustomerDateOfBirthIsCorrectSME,
		TESTManualReferralIovation,
		TESTManualReferralCIFAS,
		TESTManualReferralFraudScore,
		TESTCustomerNameIsCorrect,
		TESTMobilePhoneIsUnique,
		TESTApplicantIsNotMinor,
		TESTBankAccountIsValid,
		TESTEquifaxCreditBureauDataIsAvailable,
		TESTHomePhoneIsAcceptable,
		TESTBusinessPaymentScoreIsAcceptable,
		TESTBusinessIsCurrentlyTrading,
		TESTBusinessBureauDataIsAvailable,
		TESTMainApplicantMatchesBusinessBureauData,
		TESTBusinessPerformanceScoreIsAcceptaple,
		TESTMainApplicantDurationAcceptable,
		TESTNumberOfDirectorsMatchesBusinessBureauData,
		TESTBusinessDateOfIncorporationAcceptable,
		TESTNoCheck,
		TESTTooManyLoansAtAddress,
		TESTGuarantorNamesMatchBusinessBureauData,
		TESTBlacklistSME,
		TESTGeneralManualVerification,
		TESTDoNotRelend,
		TESTFraudBlacklist,
        TESTApplicantHasPoorRelationshipWithWonga
	}
}
