using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
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
			HomePhone = "0287000000";
			WorkPhone = "0287000010";
		}
	}
}
