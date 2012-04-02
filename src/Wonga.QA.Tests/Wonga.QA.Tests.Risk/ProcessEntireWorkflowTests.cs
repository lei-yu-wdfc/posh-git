using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk
{
		//"CustomerIsEmployedVerification",
		//"MonthlyIncomeVerification",
		//"ApplicationTermNotLessThan4DaysVerification",
		//"MobilePhoneIsUniqueVerification",
		//"BlackListVerification",
		//"IovationAutoReviewVerification",
		//"IovationVerification",
		//"DoNotRelendVerification",
		//"FraudBlacklistVerification",
		//"ReputationPredictionVerification",
		//"CreditBureauDataIsAvailableVerification",
		//"CustomerNameIsCorrectVerification",
		//"ApplicantIsSolventNoticeVerification",
		//"ApplicantIsSolventVerification",
		//"CustomerIsAliveVerification",
		//"RepaymentPredictionVerification",
		//"BankAccountIsValidVerification"

	[Pending]
	class ProcessEntireWorkflowTests
	{
		private string _surname;
		private string _forename;
		private string _middleName;
		private string _nationalNumber;
		private Date _dateOfBirth;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						_surname = "ESSACK";
						_forename = "ANITHA";
						_middleName = "DEVI";
						_nationalNumber = "5712190106083";
						_dateOfBirth = new Date(new DateTime(1957, 12, 19));
					}
					break;
				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}

			//Turn off iovation mock and AHV
		}

		[Test, AUT(AUT.Za), Pending]
		public void ProcessEntireWorkflowL0Accepted()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer("Wonga")
				//For credit bureau data
				.WithForename(_forename)
				.WithMiddleName(_middleName)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				//For default reputation score
				.WithPostcodeInAddress(Get.GetPostcode().ToString())
				.Build();

			var application = ApplicationBuilder.New(customer)
				//For high repayment prediction
				.WithLoanAmount(200)
				.WithLoanTerm(10)
				.WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.WithIovationBlackBox("Accept")
				.Build();
		}

		[Test, Pending]
		public void GenNID()
		{
			var nid = Get.GetNationalNumber();
		}
		
	}
}
