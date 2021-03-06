using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
    public partial class AddPaymentCardCommand
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
			IssueNo = "123";
			SecurityCode = "123";
			IsCreditCard = false;
			IsPrimary = true;
		}
    }
}
