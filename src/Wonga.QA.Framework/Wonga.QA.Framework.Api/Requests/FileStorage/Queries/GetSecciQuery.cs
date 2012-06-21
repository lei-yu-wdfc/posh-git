using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries
{
    /// <summary> Wonga.FileStorage.Queries.GetSecci </summary>
    [XmlRoot("GetSecci")]
    public partial class GetSecciQuery : ApiRequest<GetSecciQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
