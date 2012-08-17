using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.PayLater.Uk
{
    public partial class SavePayLaterCustomerDetailsPayLaterUkCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            DateOfBirth = Get.GetDoB();
            Email = Get.RandomEmail();
            Title = Get.RandomEnum<TitleEnum>();
            Forename = Get.GetName();
            Surname = Get.GetName();
            MobilePhone = Get.GetMobilePhone();
        }
    }
}