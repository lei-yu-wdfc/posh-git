using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SaveOrganisationDetailsCommand
    {
        public override void Default()
        {
            OrganisationId = Data.GetId();
            OrganisationName = "Test Organisation";
            RegisteredNumber = Data.RandomInt(1, 99999999).ToString().PadLeft(8,'0');
        }
    }
}
