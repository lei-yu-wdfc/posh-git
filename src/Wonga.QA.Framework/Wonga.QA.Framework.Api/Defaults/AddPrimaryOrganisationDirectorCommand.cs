using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.ContactManagement.Commands
{
    public partial class AddPrimaryOrganisationDirectorCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            Email = Get.RandomEmail();
            Forename = Get.GetName();
            Surname = Get.GetName();
            OrganisationId = Get.GetId();
            Title = Get.RandomEnum<TitleEnum>();
        }
    }
}
