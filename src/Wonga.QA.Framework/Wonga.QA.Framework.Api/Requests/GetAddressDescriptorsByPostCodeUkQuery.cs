using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Address.Queries.Uk.GetAddressDescriptorsByPostCode </summary>
    [XmlRoot("GetAddressDescriptorsByPostCode")]
    public partial class GetAddressDescriptorsByPostCodeUkQuery : ApiRequest<GetAddressDescriptorsByPostCodeUkQuery>
    {
        public Object Postcode { get; set; }
        public Object CountryCode { get; set; }
    }
}
