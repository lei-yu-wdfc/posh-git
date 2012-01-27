using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetUserLocation")]
    public class GetUserLocationQuery : ApiRequest<GetUserLocationQuery>
    {
        public Object IpAddress { get; set; }
    }
}
