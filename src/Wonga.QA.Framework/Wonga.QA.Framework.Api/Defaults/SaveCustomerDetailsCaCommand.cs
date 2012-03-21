using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SaveCustomerDetailsCaCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            Title = Get.RandomEnum<TitleEnum>();
            Forename = Get.GetName();
            Surname = Get.GetName();
            Email = Get.RandomEmail();
            DateOfBirth = Get.GetDoB();
            Gender = Get.RandomEnum<GenderEnum>();
            HomePhone = "0210000000";
            WorkPhone = "0210000000";
            NationalNumber = "000000000";
        }
    }
}