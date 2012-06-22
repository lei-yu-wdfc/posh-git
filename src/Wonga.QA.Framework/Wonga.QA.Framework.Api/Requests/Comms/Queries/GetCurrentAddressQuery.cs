using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
    /// <summary> Wonga.Comms.Queries.GetCurrentAddress </summary>
    [XmlRoot("GetCurrentAddress")]
    public partial class GetCurrentAddressQuery : ApiRequest<GetCurrentAddressQuery>
    {
        public Object AccountId { get; set; }
    }
}
