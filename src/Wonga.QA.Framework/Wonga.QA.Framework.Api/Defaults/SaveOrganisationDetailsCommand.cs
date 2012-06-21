using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.ContactManagement.Commands
{
    public partial class SaveOrganisationDetailsCommand
    {
        public override void Default()
        {
            OrganisationId = Get.GetId();
            OrganisationName = "Test Organisation";
            RegisteredNumber = Get.RandomInt(1, 99999999).ToString().PadLeft(8,'0');
        }
    }
}
