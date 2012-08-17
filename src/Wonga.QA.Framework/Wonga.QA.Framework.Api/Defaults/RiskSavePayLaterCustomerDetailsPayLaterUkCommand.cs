using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    public partial class RiskSavePayLaterCustomerDetailsPayLaterUkCommand 
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            Forename = Get.GetName();
            Surname = Get.GetName();
            Email = Get.RandomEmail();
            DateOfBirth = Get.GetDoB();           
            MobilePhone = "0210000000";
        }
    }
}