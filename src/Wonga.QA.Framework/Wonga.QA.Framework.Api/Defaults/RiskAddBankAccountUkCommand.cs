using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk
{
	public partial class RiskAddBankAccountUkCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			BankAccountId = Get.GetId();
			BankName = "ABBEY";
			AccountNumber = Get.GetBankAccountNumber();
		}
	}
}
