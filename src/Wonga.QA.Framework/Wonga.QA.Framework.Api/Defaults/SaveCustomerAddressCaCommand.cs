using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SaveCustomerAddressCaCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            AddressId = Data.GetId();
            AtAddressFrom = DateTime.Today.AddYears(-4).ToDate(DateFormat.Date);
            CountryCode = CountryCodeEnum.CA;
            County = Data.RandomString(10);
            HouseNumber = Data.RandomInt(1, 1000);
            Postcode = "K0A0A0";
            Street = "Street";
            Town = "City";
            Province = ProvinceEnum.ON;
        }
    }
}