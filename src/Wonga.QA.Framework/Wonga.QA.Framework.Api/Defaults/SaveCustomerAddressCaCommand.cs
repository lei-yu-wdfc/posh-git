using System;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Ca
{
    public partial class SaveCustomerAddressCaCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            AddressId = Get.GetId();
            AtAddressFrom = DateTime.Today.AddYears(-4).ToDate(DateFormat.Date);
            CountryCode = CountryCodeEnum.CA;
            County = Get.RandomString(10);
            HouseNumber = Get.RandomInt(1, 1000);
            Postcode = "K0A0A0";
            Street = "Street";
            Town = "City";
            Province = ProvinceEnum.ON;
        }
    }
}