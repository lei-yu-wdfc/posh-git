using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class SaveCustomerDetailsUkCommand
	{
		public override void Default()
		{
			AccountId = Get.GetId();
			DateOfBirth = Get.GetDoB();
			Email = Get.RandomEmail();
			Title = Get.RandomEnum<TitleEnum>();
			Forename = Get.GetName();
			MiddleName = Get.GetName();
			Surname = Get.GetName();
			Gender = Get.RandomEnum<GenderEnum>();
			HomePhone = "0210000000";
			WorkPhone = "0210000000";
		}
	}
}
