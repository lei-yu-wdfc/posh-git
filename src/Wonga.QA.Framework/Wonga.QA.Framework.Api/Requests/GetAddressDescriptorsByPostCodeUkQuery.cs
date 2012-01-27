using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetAddressDescriptorsByPostCode")]
    public class GetAddressDescriptorsByPostCodeUkQuery : ApiRequest<GetAddressDescriptorsByPostCodeUkQuery>
    {
        public Object Postcode { get; set; }
        public Object CountryCode { get; set; }
    }
}
