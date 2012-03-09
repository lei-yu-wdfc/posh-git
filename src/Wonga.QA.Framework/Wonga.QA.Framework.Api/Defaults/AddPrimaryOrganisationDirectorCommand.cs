using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class AddPrimaryOrganisationDirectorCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            Email = Data.RandomEmail();
            Forename = Data.GetName();
            Surname = Data.GetName();
            OrganisationId = Data.GetId();
            Title = Data.RandomEnum<TitleEnum>();
        }
    }
}
