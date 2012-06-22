using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
	public partial class RiskCreateFixedTermLoanApplicationCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			ApplicationId = Get.GetId();
			PaymentCardId = Get.GetId();
			BankAccountId = Get.GetId();
			Currency =
				Config.AUT == AUT.Uk ? CurrencyCodeEnum.GBP :
				Config.AUT == AUT.Za ? CurrencyCodeEnum.ZAR :
				Config.AUT == AUT.Ca ? CurrencyCodeEnum.CAD :
				Config.AUT == AUT.Wb ? CurrencyCodeEnum.GBP : Config.Throw<CurrencyCodeEnum>();
			PromiseDate = Get.GetPromiseDate();
			LoanAmount = Get.GetLoanAmount();
		}
	}
}
