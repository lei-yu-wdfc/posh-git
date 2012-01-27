using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetSecci")]
    public class GetSecciQuery : ApiRequest<GetSecciQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
