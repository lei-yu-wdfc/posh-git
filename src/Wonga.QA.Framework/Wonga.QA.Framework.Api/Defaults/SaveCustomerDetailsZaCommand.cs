using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SaveCustomerDetailsZaCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            Title = Data.RandomEnum<TitleEnum>();
            Forename = Data.GetName();
            Surname = Data.GetName();
            Email = Data.GetEmail();
            DateOfBirth = Data.GetDoB();
            Gender = Data.RandomEnum<GenderEnum>();
            HomeLanguage = Data.RandomEnum<LanguageZaEnum>();
            HomePhone = "0210000000";
            WorkPhone = "0210000000";
            MarriedInCommunityProperty = 0;
            NationalNumber = Data.GetNIN((Date)DateOfBirth, (GenderEnum)Gender == GenderEnum.Female);

            if ((GenderEnum)Gender != GenderEnum.Female)
                MaidenName = null;
        }
    }
}