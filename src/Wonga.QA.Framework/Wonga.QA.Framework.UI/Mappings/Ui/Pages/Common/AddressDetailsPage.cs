using System;

namespace Wonga.QA.Framework.UI.Mappings.Pages
{
    public sealed class AddressDetailsPage
    {
        public String FormId { get; set; }
        public String Postcode { get; set; }
        public String PostcodeInForm { get; set; }
        public String PostcodeLookup { get; set; }
        public String PostcodeErrorForm { get; set; }
        public String HouseNumber { get; set; }
        public String HouseNumberErrorForm { get; set; }
        public String District { get; set; }
        public String County { get; set; }
        public String CountyErrorForm { get; set; }
        public String Street { get; set; }
        public String StreetErrorForm { get; set; }
        public String Town { get; set; }
        public String TownErrorForm { get; set; }
        public String AddressPeriod { get; set; }
        public String AddressPeriodErrorForm { get; set; }
        public String AddressOptions { get; set; }
        public String PostOfficeBox { get; set; }
        public String FlatErrorForm { get; set; }

        public String LookupButton { get; set; }
        public String NextButton { get; set; }
        public String AddressOptionsWrapper { get; set; }
        public String PostcodeValid { get; set; }
        
    }
}
