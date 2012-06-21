using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Za
{
    public partial class SaveCustomerDetailsZaCommand
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
            HomeLanguage = Get.RandomEnum<LanguageZaEnum>();
            HomePhone = "0210000000";
            WorkPhone = "0210000000";
            MarriedInCommunityProperty = 0;
            NationalNumber = Get.GetNationalNumber((Date)DateOfBirth, (GenderEnum)Gender == GenderEnum.Female);

            if ((GenderEnum)Gender != GenderEnum.Female)
                MaidenName = null;
        }
    }
}