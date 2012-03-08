using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SaveCustomerDetailsCaCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            Title = Data.RandomEnum<TitleEnum>();
            Forename = Data.GetName();
            Surname = Data.GetName();
            Email = Data.RandomEmail();
            DateOfBirth = Data.GetDoB();
            Gender = Data.RandomEnum<GenderEnum>();
            HomePhone = "0210000000";
            WorkPhone = "0210000000";
            NationalNumber = "000000000";
        }
    }
}