using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class SaveCustomerAddressUkCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			AddressId = Get.GetId();
			AtAddressFrom = DateTime.Today.AddYears(-4).ToDate(DateFormat.Date);
			CountryCode = CountryCodeEnum.UK;
			County = Get.RandomString(10);
			HouseNumber = Get.RandomInt(1, 1000);
			Postcode = "SW6 6PN";
			Street = "Street";
			Town = "Town";
		}
	}
}