using System;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Za
{
    public partial class SaveCustomerAddressZaCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            AddressId = Get.GetId();
            AtAddressFrom = DateTime.Today.AddYears(-4).ToDate(DateFormat.Date);
            CountryCode = CountryCodeEnum.ZA;
            County = Get.RandomString(10);
            HouseNumber = Get.RandomInt(1, 1000);
            Postcode = "0300";
            Street = "Street";
            Town = "City";
        }
    }
}