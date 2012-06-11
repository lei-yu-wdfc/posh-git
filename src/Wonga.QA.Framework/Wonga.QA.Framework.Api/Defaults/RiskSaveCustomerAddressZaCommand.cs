using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class RiskSaveCustomerAddressZaCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			AddressId = Get.GetId();
			CountryCode = CountryCodeEnum.ZA;
			County = Get.RandomString(10);
			HouseNumber = Get.RandomInt(1, 1000);
			Postcode = "0300";
			Street = "Street";
			Town = "City";
		}
	}
}