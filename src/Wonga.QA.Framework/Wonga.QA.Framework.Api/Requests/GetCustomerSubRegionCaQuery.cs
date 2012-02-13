using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetCustomerSubRegion")]
    public partial class GetCustomerSubRegionCaQuery : ApiRequest<GetCustomerSubRegionCaQuery>
    {
        public Object AccountId { get; set; }
    }
}
