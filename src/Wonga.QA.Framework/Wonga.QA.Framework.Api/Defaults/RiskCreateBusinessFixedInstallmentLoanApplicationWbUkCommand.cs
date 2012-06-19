using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class RiskCreateBusinessFixedInstallmentLoanApplicationWbUkCommand 
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			ApplicationId = Get.GetId();
			Currency = CurrencyCodeEnum.GBP;
			LoanAmount = 10000;
			BusinessBankAccountId = Get.GetId();
			BusinessPaymentCardId = Get.GetId();
			MainApplicantPaymentCardId = Get.GetId();
			MainApplicantBankAccountId = Get.GetId();
			OrganisationId = Get.GetId();
		}
	}
}
