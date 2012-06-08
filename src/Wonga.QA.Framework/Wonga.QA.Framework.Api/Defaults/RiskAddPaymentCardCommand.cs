using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class RiskAddPaymentCardCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			PaymentCardId = Get.GetId();
			CardType = "VISA";
			Number = "4444333322221111";
			HolderName = "Test Holder";
			StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth);
			ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth);
			SecurityCode = "123";
		}
	}
}
