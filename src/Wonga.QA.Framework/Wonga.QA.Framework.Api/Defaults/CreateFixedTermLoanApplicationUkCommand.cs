using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class CreateFixedTermLoanApplicationUkCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			ApplicationId = Get.GetId();
			PaymentCardId = Get.GetId();
			BankAccountId = Get.GetId();
			Currency = CurrencyCodeEnum.GBP;
			PromiseDate = DateTime.Today.AddDays(10).ToDate(DateFormat.Date);
			LoanAmount = 100.0m;
		}
	}
}