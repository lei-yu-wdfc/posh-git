using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries.Ca
{
    /// <summary> Wonga.Comms.Queries.Ca.GetCustomerSubRegion </summary>
    [XmlRoot("GetCustomerSubRegion")]
    public partial class GetCustomerSubRegionCaQuery : ApiRequest<GetCustomerSubRegionCaQuery>
    {
        public Object AccountId { get; set; }
    }
}
