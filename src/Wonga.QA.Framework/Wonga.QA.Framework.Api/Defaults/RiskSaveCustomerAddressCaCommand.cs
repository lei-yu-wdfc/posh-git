using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class RiskSaveCustomerAddressCaCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			AddressId = Get.GetId();
			CountryCode = CountryCodeEnum.CA;
			County = Get.RandomString(10);
			HouseNumber = Get.RandomInt(1, 1000);
			Postcode = "K0A0A0";
			Street = "Street";
			Town = "City";
			Province = ProvinceEnum.ON;
		}
	}
}