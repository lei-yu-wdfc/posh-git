using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SaveCustomerAddress")]
    public class SaveCustomerAddressCaCommand : ApiRequest<SaveCustomerAddressCaCommand>
    {
        public Object AddressId { get; set; }
        public Object AccountId { get; set; }
        public Object Flat { get; set; }
        public Object HouseNumber { get; set; }
        public Object HouseName { get; set; }
        public Object Street { get; set; }
        public Object District { get; set; }
        public Object Town { get; set; }
        public Object County { get; set; }
        public Object Postcode { get; set; }
        public Object CountryCode { get; set; }
        public Object AtAddressFrom { get; set; }
        public Object AtAddressTo { get; set; }
        public Object Province { get; set; }
        public Object PoBox { get; set; }
    }
}
