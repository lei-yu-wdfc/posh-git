using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
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

    public partial class SavePaymentCardBillingAddressCommand
    {
         public override void Default()
         {
             AddressLine1 = Get.RandomString(Get.RandomInt(10,30));
             AddressLine2 = Get.RandomString(Get.RandomInt(10, 20));
             CountryCode = "UK";
             County = Get.RandomString(Get.RandomInt(5, 15));
             PostCode = Get.RandomString(Get.RandomInt(3, 6));
             Town = Get.RandomString(Get.RandomInt(5, 15));
         }
    }
}
