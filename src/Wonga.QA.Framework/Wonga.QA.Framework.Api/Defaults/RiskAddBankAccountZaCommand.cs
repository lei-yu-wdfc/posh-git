using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class RiskAddBankAccountZaCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			BankAccountId = Get.GetId();
			AccountNumber = Get.GetBankAccountNumber();
			BankName = "ABSA";			
		}
	}
}
