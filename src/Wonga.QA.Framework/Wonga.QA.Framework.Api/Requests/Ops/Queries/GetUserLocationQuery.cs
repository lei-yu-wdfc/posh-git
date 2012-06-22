using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
    /// <summary> Wonga.Ops.Queries.GetUserLocation </summary>
    [XmlRoot("GetUserLocation")]
    public partial class GetUserLocationQuery : ApiRequest<GetUserLocationQuery>
    {
        public Object IpAddress { get; set; }
    }
}
