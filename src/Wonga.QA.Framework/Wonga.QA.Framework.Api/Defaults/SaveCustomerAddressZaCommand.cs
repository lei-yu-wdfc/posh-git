using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SaveCustomerAddressZaCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            AddressId = Data.GetId();
            AtAddressFrom = DateTime.Today.AddYears(-4).ToDate(DateFormat.Date);
            CountryCode = CountryCodeEnum.ZA;
            County = Data.RandomString(10);
            HouseNumber = Data.RandomInt(1, 1000);
            Postcode = "0300";
            Street = "Street";
            Town = "City";
        }
    }
}