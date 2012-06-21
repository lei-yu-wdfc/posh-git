using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
    /// <summary> Wonga.Ops.Queries.GetAccount </summary>
    [XmlRoot("GetAccount")]
    public partial class GetAccountQuery : ApiRequest<GetAccountQuery>
    {
        public Object Login { get; set; }
        public Object Password { get; set; }
    }
}
