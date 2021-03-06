﻿
using System;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Wb.Uk
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
			ApplicationDate = Get.GetApplicationDate();
		}
	}
}
