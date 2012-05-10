using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Ops.Queries.GetMigratedStatus </summary>
    [XmlRoot("GetMigratedStatus")]
    public partial class GetMigratedStatusQuery : ApiRequest<GetMigratedStatusQuery>
    {
        public Object Login { get; set; }
    }
}
