using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetSecci")]
    public partial class GetSecciQuery : ApiRequest<GetSecciQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
