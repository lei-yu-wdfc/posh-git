using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetCustomerSubRegion")]
    public class GetCustomerSubRegionCaQuery : ApiRequest<GetCustomerSubRegionCaQuery>
    {
        public Object AccountId { get; set; }
    }
}
