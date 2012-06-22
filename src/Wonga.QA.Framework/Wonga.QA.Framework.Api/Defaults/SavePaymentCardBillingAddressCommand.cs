using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	public partial class SavePaymentCardBillingAddressCommand
	{
		public override void Default()
		{
			Flat = Get.RandomString(Get.RandomInt(1,3));
			HouseName = Get.RandomBoolean() ? Get.RandomString(5, 20) : null;
			HouseNumber = HouseName != null ? null : Get.RandomString(1, 5);
			Street = Get.RandomString(5, 25);
			District = Get.RandomBoolean() ? Get.RandomString(5, 15) : null;
			CountryCode = "UK";
			County = Get.RandomString(Get.RandomInt(5, 15));
			PostCode = Get.RandomString(Get.RandomInt(3, 6));
			Town = Get.RandomString(Get.RandomInt(5, 15));
		}
	}
}