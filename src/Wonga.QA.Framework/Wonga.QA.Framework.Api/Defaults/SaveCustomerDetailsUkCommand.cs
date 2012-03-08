using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
	public partial class SaveCustomerDetailsUkCommand
	{
		public override void Default()
		{
			AccountId = Data.GetId();
			DateOfBirth = Data.GetDoB();
			Email = Data.RandomEmail();
			Title = Data.RandomEnum<TitleEnum>();
			Forename = Data.GetName();
			MiddleName = Data.GetName();
			Surname = Data.GetName();
			Gender = Data.RandomEnum<GenderEnum>();
			HomePhone = "0210000000";
			WorkPhone = "0210000000";
		}
	}
}
